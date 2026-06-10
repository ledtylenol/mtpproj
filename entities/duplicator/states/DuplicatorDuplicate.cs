using Godot;
using System;

[GlobalClass]
public partial class DuplicatorDuplicate : State
{
	[Export]
	public Duplicator Duplicator { get; set; }

	[Export]
	public Timer DuplicateTimer { get; set; }

	private double timeLeft = 0f;
	public override void OnEnter()
	{
		timeLeft = 1f;

		DuplicateTimer.Start(GD.RandRange(2f, 4f));
	}

	public override void OnExit()
	{
	}

	public override void PhysicsTick(double delta)
	{
		timeLeft -= delta;
		if (timeLeft <= 0f)
		{
			Spawn();
			return;
		}
		float weight = -(float)delta * Duplicator.Friction;
		Duplicator.CurrentSpeed = Mathf.Lerp(Duplicator.CurrentSpeed, 0f, 1.0f - Mathf.Exp(weight));
		Duplicator.Move(delta);
	}

	public override void Tick(double delta)
	{
	}

	private void Spawn()
	{
		Duplicator.DuplicateSpawn();
		EmitSignalTransitioned("follow");
	}
}
