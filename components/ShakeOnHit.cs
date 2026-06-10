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

	[Export]
	public bool ShakeOnDeath { get; set; } = true;

	public override void _Ready()
	{
		base._Ready();
		Health.Damaged += Shake;
	}

	private void Shake(Entity entity, HitBox source, float damage)
	{
		if (!Active) return;
		if (Health.CurrentHealth == 0 && !ShakeOnDeath) return;
		if ((bool)Shaker.Get("is_playing")) Shaker.Call("force_stop_shake");
		Shaker.Call("play_shake");
	}
}
