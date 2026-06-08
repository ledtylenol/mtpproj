using Godot;
using System;

[GlobalClass]
public abstract partial class State : Node
{
	public Entity Entity { get; set; }
	public override void _Ready()
	{
		base._Ready();
		SetProcess(false);
		SetPhysicsProcess(false);
		Entity = Owner as Entity;
	}
	[Signal]
	public delegate void TransitionedEventHandler(string to);

	public abstract void OnEnter();
	public abstract void OnExit();

	public abstract void Tick(double delta);
	public abstract void PhysicsTick(double delta);
}
