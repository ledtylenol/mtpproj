using Godot;
using System;

[GlobalClass]
public partial class PlayerMove : PlayerState
{
	public override void OnEnter()
	{
		GD.Print("Entered move");
	}

	public override void OnExit()
	{
	}

	public override void PhysicsTick(double delta)
	{
		Player.ProcessInputs();
		if (Direction.IsZeroApprox())
		{
			EmitSignalTransitioned("idle");
			return;
		}
		Velocity = Velocity.MoveToward(Direction * Player.MoveSpeed, (float)delta * Player.Acceleration);
		Player.Move(delta);
	}

	public override void Tick(double delta)
	{
	}

}
