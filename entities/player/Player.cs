using Godot;
using System;

[GlobalClass]
public partial class Player : Entity
{


	public Vector2 Direction { get; set; }
	public void ProcessInputs()
	{
		Direction = Input.GetVector("a", "d", "w", "s");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		StateMachine.PhysicsTick(delta);
	}
	public override void _Process(double delta)
	{
		base._Process(delta);
		StateMachine.Tick(delta);
	}
}
