using Godot;
using System;

[GlobalClass]
public partial class EraseOnDeath : Node
{
	[Export]
	private Health Health { get; set; }

	[Export]
	private float HideTime { get; set; } = 0f;
	public override void _Ready()
	{
		base._Ready();
		Health.Died += Die;
	}

	async private void Die(Entity entity)
	{
		entity.Hide();
		if (HideTime > 0f)
			GetTree().CreateTimer(HideTime).Timeout += entity.QueueFree;
		else entity.QueueFree();

	}
}
