using Godot;
using System;

public partial class HitTest : Entity
{
	[Export]
	private Health Health { get; set; }

	public override void _Ready()
	{
		base._Ready();
		Health.Died += (e) => QueueFree();
	}
}
