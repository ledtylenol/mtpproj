using Godot;
using System;

/* utility to access player easier */
[GlobalClass]
public abstract partial class PlayerState : State
{
	[Export]
	private Player Player { get; set; }

	public override void _Ready()
	{
		base._Ready();
		Player ??= Owner as Player;
		GD.Print($"{Player.Name}");
	}
}
