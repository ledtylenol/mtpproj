using Godot;
using System;

[GlobalClass]
public abstract partial class JumperState : State
{
	protected Jumper Jumper { get; set; }

	public override void _Ready()
	{
		base._Ready();
		Jumper = Owner as Jumper;
	}
}
