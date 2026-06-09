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
	protected uint BulletCount { get; set; }
	[Export]
	protected float Spread { get; set; }
	[Export]
	protected float Separation { get; set; }
	[Export]
	protected float Range { get; set; }

	[Export]
	public float Cooldown { get; set; }

	private float RealCooldown { get; set; } = 0f;

	[Signal]
	public delegate void ShotEventHandler();
	public override void _Ready()
	{
		base._Ready();
		Entity ??= Owner as Entity;
	}

	public void Shoot()
	{
		if (RealCooldown <= 0f)
		{
			RealCooldown = GetCooldown();

			var dir = GetDir();
			var translation = dir.Rotated(Mathf.Pi / 2f) * Separation;
			var spread = GetSpread();
			var bulletCount = GetBulletCount();
			for (int i = 0; i < bulletCount; i++)
			{
				var instance = BulletScene.Instantiate<Bullet>();
				instance.Range = GetRange();
				instance.Root = this;
				instance.Transform = GlobalTransform.Translated(translation * i - translation * (bulletCount / 2) + translation / 2f);
				instance.Direction = dir.Rotated(spread * i - spread * (bulletCount / 2) + spread / 2f);
				Global.Single.Spawn(instance);
			}
			EmitSignalShot();
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		//TODO use timer node?
		RealCooldown -= (float)delta;
	}

	public abstract Vector2 GetDir();
	public abstract float GetCooldown();
	public abstract float GetBulletCount();
	public abstract float GetSpread();
	public abstract float GetRange();
}
