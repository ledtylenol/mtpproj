using Godot;
using System;
using System.Threading;

[GlobalClass]
public partial class Ghosts : Node
{
	[Export]
	public Sprite2D NodeToCopy { get; set; }

	[Export]
	public float SpawnInterval { get; set; }

	[Export]
	public bool Active { get; set; } = false;

	private float RealCooldown { get; set; }

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		RealCooldown -= (float)delta;

		if (Active && RealCooldown <= 0f)
		{
			SpawnGhost();
			RealCooldown = SpawnInterval;
		}
	}

	private void SpawnGhost()
	{
		var sprite = new Ghost(0.5f)
		{
			Texture = NodeToCopy.Texture,
			Transform = NodeToCopy.GlobalTransform
		};

		Global.Single.Spawn(sprite);
	}
}
