using Godot;
using System;

[GlobalClass]
public partial class ShakeOnHit : Node
{
	[Export]
	public Health Health { get; set; }

	[Export]
	public Node2D Shaker { get; set; }

	[Export]
	public bool Active { get; set; } = true;

	public override void _Ready()
	{
		base._Ready();
		Health.Damaged += Shake;
	}

	private void Shake(Entity entity, HitBox source, float damage)
	{
		if (!Active) return;
		Shaker.Call("play_shake");
	}
}
