using Godot;
using System;

[GlobalClass]
public partial class Entity : CharacterBody2D
{
	[Export]
	public float MoveSpeed { get; set; }

	[Export]
	public float Acceleration { get; set; }

	[Export]
	public float Friction { get; set; }

	[Export]
	public bool CountsTowardEnemies { get; set; } = true;

	[Export]
	public StateMachine StateMachine { get; set; }

	[Export]
	public Health Health { get; set; }

	public Vector2? Normal { get; set; }
	public override void _Ready()
	{
	}

	public virtual void Move(double _delta)
	{
		MoveAndSlide();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		StateMachine?.PhysicsTick(delta);
	}
	public override void _Process(double delta)
	{
		base._Process(delta);
		StateMachine?.Tick(delta);
	}
}
