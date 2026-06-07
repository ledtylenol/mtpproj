using Godot;
using System;

[GlobalClass]
public abstract partial class BulletShooter : Node2D
{
	[Export]
	public Entity Entity { get; set; }
	[Export]
	private PackedScene BulletScene { get; set; }
	[Export]
	private uint BulletCount { get; set; }
	[Export]
	private float Spread { get; set; }
	[Export]
	private float Separation { get; set; }

	[Export]
	public float Cooldown { get; set; }

	private float RealCooldown { get; set; } = 0f;

	public override void _Ready()
	{
		base._Ready();
		Entity ??= Owner as Entity;
	}

	public void Shoot()
	{
		if (RealCooldown <= 0f)
		{
			RealCooldown = Cooldown;

			var dir = GetDir();
			var translation = dir.Rotated(Mathf.Pi / 2f) * Separation;
			for (int i = 0; i < BulletCount; i++)
			{
				var instance = BulletScene.Instantiate<Bullet>();
				instance.Root = this;
				instance.Transform = GlobalTransform.Translated(translation * i - translation * (BulletCount / 2) / 2f);
				instance.Direction = dir.Rotated(Spread * (i - BulletCount / 2));
				Global.Single.Spawn(instance);
			}
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		//TODO use timer node?
		RealCooldown -= (float)delta;
	}

	public abstract Vector2 GetDir();
}
