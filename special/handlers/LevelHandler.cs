using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LevelHandler : Node
{
	[Export]
	public Array<Enemy> PossibleEnemies = [];

	[Export]
	public ExplosionStats Stats { get; set; }

	private List<Entity> aliveEnemies = [];

	public static LevelHandler Single { get; set; }
	public float CurrentDifficulty { get; set; } = 1f;

	[Signal]
	public delegate void LevelFinishedEventHandler();


	[Signal]
	public delegate void LevelStartedEventHandler();
	public override void _Ready()
	{
		base._Ready();
		Single = this;
	}
	public void SpawnEnemies()
	{
		var totalPoints = CurrentDifficulty * 15f;
		while (totalPoints > 0f)
		{
			EmitSignalLevelStarted();
			Enemy enemy;
			var possibleEnemies = PossibleEnemies.Where((enemy) => enemy.Cost < totalPoints).ToList();
			var size = possibleEnemies.Count;
			if (size < 1) enemy = PossibleEnemies.First();
			else enemy = possibleEnemies[(int)(GD.Randi() % size)];

			var playArea = Global.World.PlayArea;
			var xRange = playArea.Size.X;
			var yRange = playArea.Size.Y;
			var pos = new Vector2((float)GD.RandRange(0f, xRange), (float)GD.RandRange(0f, yRange));

			var inst = enemy.Scene.Instantiate<Entity>();
			inst.Position = pos + playArea.Position;

			inst.Health.Died += OnEnemyDeath;

			Global.Single.Spawn(inst);
			totalPoints -= enemy.Cost;
			aliveEnemies.Add(inst);
		}
	}
	private void OnEnemyDeath(Entity entity)
	{
		aliveEnemies.Remove(entity);
		if (aliveEnemies.Count == 0)
		{
			GD.Print("All enemies are dead");
			var playArea = Global.World.PlayArea;
			var expl = new Explosion(Stats, 0, 0)
			{
				Position = playArea.Position + playArea.Size / 2f,
				ZIndex = 100
			};
			Global.Single.Spawn(expl);

			if (Global.Player.Dead) return;
			Global.Single.ChangeLevel();

			CurrentDifficulty += 0.1f;
			EmitSignalLevelFinished();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (!Global.Player.Dead && Input.IsActionJustPressed("proceed") && aliveEnemies.Count == 0) SpawnEnemies();
	}

	public void Reset()
	{
		CurrentDifficulty = 1f;
		aliveEnemies.Clear();
		EmitSignalLevelFinished();
	}
}
