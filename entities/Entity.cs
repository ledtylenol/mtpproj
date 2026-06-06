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
	public StateMachine StateMachine { get; set; }

	public Vector2? Normal { get; set; }
	public override void _Ready()
	{
	}

	public virtual void Move(double _delta)
	{
		MoveAndSlide();
	}
}
