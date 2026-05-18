using Godot;
using System;

/* utility to access player easier */
[GlobalClass]
public abstract partial class PlayerState : State
{
	[Export]
	protected Player Player { get; set; }

	protected Vector2 Direction
	{
		get => Player.Direction;
		set => Player.Direction = value;
	}

	protected Vector2 Velocity
	{

		get => Player.Velocity;
		set => Player.Velocity = value;
	}

	public override void _Ready()
	{
		base._Ready();
		Player ??= Owner as Player;
		GD.Print($"{Player.Name}");
	}
}
