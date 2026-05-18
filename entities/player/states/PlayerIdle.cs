using Godot;
using System;

[GlobalClass]
public partial class PlayerIdle : PlayerState
{
	public override void OnEnter()
	{
		GD.Print("Entered idle");
		EmitSignalTransitioned("adbc");
	}

	public override void OnExit()
	{
	}

	public override void PhysicsTick(double delta)
	{
		Player.ProcessInputs();
		if (!Direction.IsZeroApprox())
		{
			EmitSignalTransitioned("move");
			return;
		}

		Velocity = Velocity.MoveToward(Vector2.Zero, (float)delta * Player.Friction);
		Player.Move(delta);
	}

	public override void Tick(double delta)
	{
	}

}
