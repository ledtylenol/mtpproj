using Godot;
using System;

[GlobalClass]
public partial class JumperIdle : JumperState
{
	[Export]
	public Timer JumpTimer { get; set; }
	public override void OnEnter()
	{
		JumpTimer.Timeout += Jump;
	}

	public override void OnExit()
	{
		JumpTimer.Timeout -= Jump;
	}

	public override void PhysicsTick(double delta)
	{
		Jumper.Velocity = Jumper.Velocity.MoveToward(Vector2.Zero, (float)delta * Jumper.Friction);
		Jumper.Move(delta);
	}

	public override void Tick(double delta)
	{
	}

	private void Jump()
	{
		JumpTimer.Start(Jumper.Cooldown + GD.RandRange(0.5, 0.9));
		EmitSignalTransitioned("jump");
	}

}
