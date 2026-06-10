using Godot;
using System;
using System.Linq;

[GlobalClass]
public partial class DuplicatorFollow : State
{
	[Export]
	public Duplicator Duplicator { get; set; }
	[Export]
	public Area2D PlayerDetection { get; set; }
	[Export]
	public Timer DashTimer { get; set; }

	[Export]
	public Timer DuplicateTimer { get; set; }

	public override void OnEnter()
	{
	}

	public override void OnExit()
	{
	}

	public override void PhysicsTick(double delta)
	{
		float weight = -(float)delta * Duplicator.TurnSpeed;

		Duplicator.UpdateDir();
		Duplicator.CurrentSpeed = Mathf.Lerp(Duplicator.CurrentSpeed, Duplicator.MoveSpeed, 1.0f - Mathf.Exp(weight));
		Duplicator.Direction = Duplicator.Direction.Slerp(Duplicator.PlayerDir, 1.0f - Mathf.Exp(weight));
		Duplicator.Move(delta);
		var bodies = PlayerDetection.GetOverlappingBodies();
		if (bodies.Count > 0 && DashTimer.TimeLeft <= 0f)
		{
			EmitSignalTransitioned("Enraged");
		}
		if (DuplicateTimer.TimeLeft <= 0f)
			EmitSignalTransitioned("Duplicate");
	}

	public override void Tick(double delta)
	{
	}
}
