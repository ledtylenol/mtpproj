using Godot;
using System;

[GlobalClass]
public partial class Player : Entity
{
	[Export]
	private ExplosionStats DeathExplosionStats { get; set; }

	[Export]
	ShakeOnHit Shake { get; set; }

	[Export]
	public Sprite2D Sprite { get; set; }

	[Signal]
	public delegate void DiedEventHandler();

	public Vector2 Direction { get; set; }
	public bool Dead { get; set; } = false;

	public override void _Ready()
	{
		base._Ready();
		Global.Player = this;
		Health.Died += SetDead;
	}
	public void ProcessInputs()
	{
		Direction = Input.GetVector("a", "d", "w", "s");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Sprite.GlobalPosition = GlobalPosition.Snapped(new Vector2(1f, 1f));
	}
	public override void _Process(double delta)
	{
		base._Process(delta);
		StateMachine.Tick(delta);
	}

	private void SetDead(Entity _entity)
	{
		Dead = true;
		Shake.Active = false;
	}

	public void UnDie()
	{
		Dead = false;
		Shake.Active = true;
		Visible = true;
		Velocity = Vector2.Zero;
		Position = Vector2.Zero;
		ResetPhysicsInterpolation();
	}
	public void Die()
	{

		EmitSignalDied();
	}
}
