using Godot;
using System;

[GlobalClass]
public partial class PlayerIdle : PlayerState
{
	public override void OnEnter()
	{
		GD.Print("Entered idle");
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

}
