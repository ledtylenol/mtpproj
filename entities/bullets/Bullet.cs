using Godot;
using System;

[GlobalClass]
public partial class Bullet : Entity
{
	[Export]
	public HitBox HitBox { get; set; }

	public BulletShooter Root { get; set; }
	public Vector2 Direction { get; set; }

	public override void _Ready()
	{
		base._Ready();
		//pass the owner to the hitbox for ignore purposes
		HitBox.Entity = Root.Entity;
		HitBox.OutOfHits += (hb) => QueueFree();
		Global.Single.LevelChanged += HandleLevelChanged;
	}

	public override void _PhysicsProcess(double delta)
	{
		Move(delta);
	}

	public override void Move(double delta)
	{

		var wishVel = Direction * MoveSpeed;
		if (Acceleration > 0f) Velocity = Velocity.MoveToward(wishVel, (float)delta * Acceleration);
		else Velocity = wishVel;

		MoveAndSlide();
	}
	private void HandleLevelChanged()
	{
		GD.Print("LEVEL CHANGED");
		QueueFree();
	}
}
