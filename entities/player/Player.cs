using Godot;
using System;

[GlobalClass]
public partial class Player : Entity
{
	[Export]
	private ExplosionStats DeathExplosionStats { get; set; }
	[Export]
	private Health Health { get; set; }

	[Export]
	ShakeOnHit Shake { get; set; }

	[Export]
	Sprite2D Sprite { get; set; }
	[Signal]
	public delegate void DiedEventHandler();

	public Vector2 Direction { get; set; }
	public bool Dead { get; set; } = false;

	public override void _Ready()
	{
		base._Ready();
		Health.Died += Die;
	}
	public void ProcessInputs()
	{
		Direction = Input.GetVector("a", "d", "w", "s");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		StateMachine.PhysicsTick(delta);
		Sprite.GlobalPosition = GlobalPosition.Snapped(new Vector2(1f, 1f));
	}
	public override void _Process(double delta)
	{
		base._Process(delta);
		StateMachine.Tick(delta);
	}

	private void Die(Entity _entity)
	{
		Dead = true;
		Shake.Active = false;

		EmitSignalDied();
	}
}
