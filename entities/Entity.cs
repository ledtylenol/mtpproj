using Godot;
using System;

[GlobalClass]
public partial class Entity : CharacterBody2D
{
	public override void _Ready()
	{
		GD.Print("test");
	}
}
