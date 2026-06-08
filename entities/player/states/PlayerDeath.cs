using Godot;
using System;

[GlobalClass]
public partial class PlayerDeath : PlayerState
{
	[Export]
	Node2D ShakerNode { get; set; }

	[Export]
	ExplosionStats ExplosionStats { get; set; }

	[Export]
	ExplosionStats FinalExplosionStats { get; set; }

	[Export]
	int ExplosionCount { get; set; }

	[Export]
	double ExplosionDelay { get; set; }

	[Export]
	double InitialExplosionDelay { get; set; }

	private Tween DeathTween { get; set; }
	public override void OnEnter()
	{
		TweenDeath();
	}

	public override void OnExit()
	{
		ShakerNode.Call("force_stop_shake");
		ShakerNode.Set("intensity", 1f);
	}

	public override void PhysicsTick(double delta)
	{
	}

	public override void Tick(double delta)
	{

		if (DeathTween.IsRunning()) return;
		if (Input.IsActionJustPressed("proceed"))
		{
			Global.World.Reset();
			EmitSignalTransitioned("idle");
		}

	}

	private void TweenDeath()
	{
		DeathTween?.Kill();
		ShakerNode.Call("play_shake");
		DeathTween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
		DeathTween.TweenInterval(InitialExplosionDelay);
		double delay = ExplosionDelay;
		for (int i = 0; i < GD.Randi() % 3 + ExplosionCount; i++)
		{
			var subt = CreateTween();
			subt.TweenCallback(SpawnExplosion(i));
			DeathTween.TweenSubtween(subt).SetDelay(delay);
		}

		DeathTween.TweenCallback(Callable.From(SpawnFinalExplosion));
		DeathTween.TweenCallback(Callable.From(Player.Hide));
		DeathTween.TweenProperty(ShakerNode, "intensity", 0f, 0.5f).From(1f);
	}

	private Callable SpawnExplosion(int i)
	{
		//Unholy partial application of doom and despair
		return Callable.From(() =>
		{
			float iReal = (float)i / ExplosionCount;
			float posOffset = iReal * 50f;

			var x = (float)GD.RandRange(-posOffset, posOffset);
			var y = (float)GD.RandRange(-posOffset, posOffset);

			var pos = new Vector2(x, y);
			var explosion = new Explosion(ExplosionStats, 1 << 1, 1 << 2)
			{
				Position = pos + Player.Position,
				Scale = new Vector2(1f + iReal, 1f + iReal)
			};

			Global.Single.Spawn(explosion);
		}
		);
	}
	private void SpawnFinalExplosion()
	{
		var explosion = new Explosion(FinalExplosionStats, 1 << 1, 1 << 2)
		{
			Transform = Player.Transform
		};
		Global.Single.Spawn(explosion);
		Player.Die();
	}

}
