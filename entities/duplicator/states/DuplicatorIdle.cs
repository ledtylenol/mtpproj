using Godot;
using System;

[GlobalClass]
public partial class DuplicatorIdle : State
{
	[Export]
	public Duplicator Duplicator { get; set; }
	[Export]
	public Timer FollowTimer { get; set; }
	public override void OnEnter()
	{
		FollowTimer.Start(0.5f);

	}

	public override void OnExit()
	{
	}

	public override void PhysicsTick(double delta)
	{
		float weight = -(float)delta * Duplicator.Friction;
		Duplicator.CurrentSpeed = Mathf.Lerp(Duplicator.CurrentSpeed, 0f, 1.0f - Mathf.Exp(weight));
		Duplicator.Move(delta);
		if (FollowTimer.TimeLeft <= 0f)
			EmitSignalTransitioned("follow");
	}

	public override void Tick(double delta)
	{
	}

}
