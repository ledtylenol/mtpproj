using Godot;
using System;

[GlobalClass]
public partial class ItemHolder : ColorRect
{
	[Export]
	public bool Active { get; set; }
	public override void _Ready()
	{
		base._Ready();
	}
}
