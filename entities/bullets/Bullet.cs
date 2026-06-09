using Godot;
using System;

[GlobalClass]
public partial class Bullet : Entity
{
	[Export]
	public HitBox HitBox { get; set; }

	public BulletShooter Root { get; set; }
	public Vector2 Direction { get; set; }
	public float Range { get; set; }

	public override void _Ready()
	{
		base._Ready();
		//pass the owner to the hitbox for ignore purposes
		HitBox.Entity = Root.Entity;
		HitBox.OutOfHits += (hb) => QueueFree();
		LevelHandler.Single.LevelFinished += HandleLevelChanged;
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
		Range -= (Velocity * (float)delta).Length();
		if (Range <= 0f) QueueFree();
	}
	private void HandleLevelChanged()
	{
		if (!IsInstanceValid(this)) return;
		QueueFree();
	}
}
