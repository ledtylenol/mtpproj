using Godot;
using System;

[GlobalClass]
public abstract partial class State : Node
{
	public override void _Ready()
	{
		base._Ready();
		SetProcess(false);
		SetPhysicsProcess(false);
	}
	[Signal]
	public delegate void TransitionedEventHandler(string to);

	public abstract void OnEnter();
	public abstract void OnExit();

	public abstract void Tick(double delta);
	public abstract void PhysicsTick(double delta);
}
