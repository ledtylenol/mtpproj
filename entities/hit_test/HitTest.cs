using Godot;
using System;

public partial class HitTest : Entity
{

	public override void _Ready()
	{
		base._Ready();
		Health.Died += (e) => QueueFree();
	}
}
