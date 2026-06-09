using Godot;
using System;

[GlobalClass]
public partial class PlayerDash : PlayerState
{
	[Export]
	public Ghosts Ghosts { get; set; }
	[Export]
	public float DashSpeed { get; set; } = 200f;

	[Export]
	public float DashDistance { get; set; } = 100f;

	private float distanceTravelled = 0f;
	private float startVel = 0f;
	public override void OnEnter()
	{

		startVel = Velocity.Length();
		Velocity = Direction * DashSpeed;
		distanceTravelled = 0f;
		Ghosts.Active = true;
	}

	public override void OnExit()
	{
		Ghosts.Active = false;
	}

	public override void PhysicsTick(double delta)
	{
		distanceTravelled += (Velocity * (float)delta).Length();
		if (distanceTravelled >= DashDistance)
		{
			if (Direction.Length() != 0f)
			{
				Velocity = Direction * startVel;
				EmitSignalTransitioned("move");
			}
			else
			{
				EmitSignalTransitioned("idle");
			}
		}
		Player.Move(delta);
	}

	public override void Tick(double delta)
	{
	}

}
