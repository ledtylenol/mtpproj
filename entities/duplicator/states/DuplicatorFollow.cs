using Godot;
using System;

[GlobalClass]
public partial class DuplicatorFollow : State
{
	[Export]
	public Duplicator Duplicator { get; set; }
	[Export]
	public Area2D PlayerDetection { get; set; }
	[Export]
	public Timer DashTimer { get; set; }
	public override void OnEnter()
	{
		GetTree().CreateTimer(GD.RandRange(2f, 4f)).Timeout += () => EmitSignalTransitioned("duplicate");
		PlayerDetection.BodyEntered += OnBodyEntered;
	}

	public override void OnExit()
	{
		PlayerDetection.BodyEntered -= OnBodyEntered;
	}

	public override void PhysicsTick(double delta)
	{
		float weight = -(float)delta * Duplicator.TurnSpeed;

		Duplicator.UpdateDir();
		Duplicator.CurrentSpeed = Mathf.Lerp(Duplicator.CurrentSpeed, Duplicator.MoveSpeed, 1.0f - Mathf.Exp(weight));
		Duplicator.Direction = Duplicator.Direction.Slerp(Duplicator.PlayerDir, 1.0f - Mathf.Exp(weight));
		Duplicator.Move(delta);
	}

	public override void Tick(double delta)
	{
	}
	private void OnBodyEntered(Node2D body)
	{
		if (body is not Player) return;
		if (DashTimer.TimeLeft > 0f) return;
		DashTimer.Start();
		EmitSignalTransitioned("Enraged");
	}
}
