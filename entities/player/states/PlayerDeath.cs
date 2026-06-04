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

	public override void OnEnter()
	{
		TweenDeath();
	}

	public override void OnExit()
	{
	}

	public override void PhysicsTick(double delta)
	{
	}

	public override void Tick(double delta)
	{
	}

	private void TweenDeath()
	{
		ShakerNode.Call("play_shake");
		var deathTween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
		deathTween.TweenInterval(InitialExplosionDelay);
		double delay = ExplosionDelay;
		for (int i = 0; i < GD.Randi() % 3 + ExplosionCount; i++)
		{
			var subt = CreateTween();
			subt.TweenCallback(Callable.From(SpawnExplosion));
			deathTween.TweenSubtween(subt).SetDelay(delay);
		}

		deathTween.TweenCallback(Callable.From(SpawnFinalExplosion));
		deathTween.TweenProperty(ShakerNode, "intensity", 0.0f, 0.5f);
	}

	private void SpawnExplosion()
	{
		var x = (float)GD.RandRange(-50f, 50f);
		var y = (float)GD.RandRange(-50f, 50f);

		var pos = new Vector2(x, y);
		var explosion = new Explosion(ExplosionStats)
		{
			Position = pos
		};

		AddChild(explosion);
	}
	private void SpawnFinalExplosion()
	{
		var explosion = new Explosion(FinalExplosionStats);

		AddChild(explosion);
	}

}
