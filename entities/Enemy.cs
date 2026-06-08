using Godot;
using System;

[GlobalClass]
public partial class Enemy : Resource
{
	[Export]
	public PackedScene Scene { get; set; }

	[Export]
	public float Cost { get; set; }
}
