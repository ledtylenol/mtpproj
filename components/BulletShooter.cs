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

			var instance = BulletScene.Instantiate<Bullet>();
			instance.Root = this;
			instance.Transform = GlobalTransform;
			instance.Direction = GetDir();
			Global.Single.Spawn(instance);
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
