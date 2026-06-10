using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LevelHandler : Node
{


	[Export]
	public Array<LevelPool> Pools = [];

	[Export]
	public Array<LevelPool> BossPools = [];

	[Export]
	public ExplosionStats Stats { get; set; }

	[Export]
	public Array<PackedScene> PowerupScenes { get; set; } = [];

	[Export]
	public ExplosionStats ItemExplosionStats { get; set; }

	public bool Active { get; set; }
	private List<Entity> aliveEnemies = [];
	private List<Entity> decorativeEnemies = [];
	private PackedScene NextPowerup = null;

	public static LevelHandler Single { get; set; }
	public float CurrentDifficulty { get; set; } = 1f;
	public uint CurrentLevel { get; set; } = 0;

	[Signal]
	public delegate void LevelFinishedEventHandler();

	[Signal]
	public delegate void LevelStartedEventHandler();

	[Signal]
	public delegate void BossFinishedEventHandler();

	[Signal]
	public delegate void BossStartedEventHandler();

	[Signal]
	public delegate void BossNextTurnEventHandler();

	[Signal]
	public delegate void BossEnemyKilledEventHandler();
	public override void _Ready()
	{
		base._Ready();
		Single = this;
	}
	public bool IsBoss()
	{
		return (CurrentLevel + 1) % 6 == 0;
	}

	public bool WillBeBoss()
	{
		return (CurrentLevel + 2) % 6 == 0;
	}
	public void SpawnBoss()
	{
		var totalPoints = CurrentDifficulty * 15f;
		Active = true;

		LevelPool pool;
		var possiblePools = BossPools.Where((pool) => pool.MinimumCost <= CurrentDifficulty && pool.MaximumCost >= CurrentDifficulty).ToList();
		var size = possiblePools.Count;
		if (size < 1) pool = possiblePools.First();
		else pool = possiblePools[(int)(GD.Randi() % size)];
		var tween = CreateTween().SetParallel();
		var delay = 0f;
		EmitSignalLevelStarted();
		EmitSignalBossStarted();
		var playArea = Global.World.PlayArea;
		var xRange = playArea.Size.X;
		var yRange = playArea.Size.Y;
		var enemies = pool.PossibleEnemies;
		CurrentDifficulty += 0.5f;
		foreach (var enemy in enemies)
		{
			var inst = enemy.Scene.Instantiate<Entity>();

			inst.Health.Died += OnEnemyDeath;
			var wrapper = new SpawnWrapper(enemy.SpawnExplosionStats, inst);

			var subtween = CreateTween();
			subtween.TweenCallback(Callable.From(() => Global.Single.Spawn(wrapper)));
			tween.TweenSubtween(subtween).SetDelay(delay);

			var pos = new Vector2((float)GD.RandRange(5f, xRange - 5f), (float)GD.RandRange(5f, yRange - 5f));

			if (enemy.RandomizePosition)
				inst.Position = pos + playArea.Position;
			if (GD.Randi() % 3 != 0)
				delay += (float)GD.RandRange(0.05, 0.25);
			aliveEnemies.Add(inst);
		}
		NextPowerup = PowerupScenes.PickRandom();
		GD.Print(PowerupScenes.Count);
	}
	public void SpawnEnemies()
	{
		var totalPoints = CurrentDifficulty * 15f;
		Active = true;


		LevelPool pool;
		var possiblePools = Pools.Where((pool) => pool.MinimumCost <= CurrentDifficulty && pool.MaximumCost >= CurrentDifficulty).ToList();
		var size = possiblePools.Count;
		if (size < 1) pool = possiblePools.First();
		else pool = possiblePools[(int)(GD.Randi() % size)];
		var tween = CreateTween().SetParallel();
		var delay = 0f;
		EmitSignalLevelStarted();
		while (totalPoints > 0f)
		{

			var playArea = Global.World.PlayArea;
			var xRange = playArea.Size.X;
			var yRange = playArea.Size.Y;
			var pos = new Vector2((float)GD.RandRange(5f, xRange - 5f), (float)GD.RandRange(5f, yRange - 5f));

			var enemy = pool.GetEnemy(totalPoints);
			if (enemy is null) break;
			var inst = enemy.Scene.Instantiate<Entity>();
			if (enemy.RandomizePosition)
				inst.Position = pos + playArea.Position;

			if (inst.CountsTowardEnemies)
				inst.Health.Died += OnEnemyDeath;
			else
				inst.Health.Died += OnDecorativeEnemyDeath;
			var wrapper = new SpawnWrapper(enemy.SpawnExplosionStats, inst);

			var subtween = CreateTween();
			subtween.TweenCallback(Callable.From(() => Global.Single.Spawn(wrapper)));
			tween.TweenSubtween(subtween).SetDelay(delay);
			if (GD.Randi() % 3 != 0)
				delay += (float)GD.RandRange(0.05, 0.25);
			totalPoints -= enemy.Cost;
			if (inst.CountsTowardEnemies)
				aliveEnemies.Add(inst);
			else decorativeEnemies.Add(inst);
		}
	}
	private void OnEnemyDeath(Entity entity)
	{
		aliveEnemies.Remove(entity);
		if (IsBoss()) EmitSignalBossEnemyKilled();
		if (aliveEnemies.Count == 0)
		{
			if (Global.Player.Dead) return;
			GD.Print("All enemies are dead");
			var playArea = Global.World.PlayArea;
			var expl = new Explosion(Stats, 0, 0, false)
			{
				Position = playArea.Position + playArea.Size / 2f,
				ZIndex = 100
			};
			Global.Single.Spawn(expl);

			if (Global.Player.Dead) return;

			CurrentDifficulty += 0.1f;
			EmitSignalLevelFinished();
			if (IsBoss())
			{
				EmitSignalBossFinished();
			}
			if (WillBeBoss())
			{
				EmitSignalBossNextTurn();
			}
			CurrentLevel++;
			Active = false;
			if (NextPowerup is null) return;


			var inst = NextPowerup.Instantiate<PowerupContainer>();
			inst.Position = Vector2.Up * 25;
			var wrapper = new SpawnWrapper(ItemExplosionStats, inst);

			GetTree().CreateTimer(GD.RandRange(0.1, 0.6)).Timeout += () => Global.Single.Spawn(wrapper);
			NextPowerup = null;
		}
	}

	private void OnDecorativeEnemyDeath(Entity entity)
	{
		decorativeEnemies.Remove(entity);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (!Global.Player.Dead && Input.IsActionJustPressed("proceed") && aliveEnemies.Count == 0) StartNewLevel();
	}

	public void Reset()
	{
		CurrentDifficulty = 1f;
		CurrentLevel = 0;
		aliveEnemies.Clear();
		decorativeEnemies.Clear();
		EmitSignalLevelFinished();
		NextPowerup = null;
	}

	private void StartNewLevel()
	{
		if ((CurrentLevel + 1) % 6 == 0) SpawnBoss();
		else SpawnEnemies();
	}
	public void SpawnEnemy(Entity enemy)
	{
		Global.Single.Spawn(enemy);
		aliveEnemies.Add(enemy);
		enemy.Health.Died += OnEnemyDeath;
	}
}
