using Godot;
using System;

[GlobalClass]
public partial class DuplicatorDuplicate : State
{
	[Export]
	public Duplicator Duplicator { get; set; }

	public override void OnEnter()
	{
		GetTree().CreateTimer(1f, false).Timeout += Spawn;
	}

	public override void OnExit()
	{
	}

	public override void PhysicsTick(double delta)
	{
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
