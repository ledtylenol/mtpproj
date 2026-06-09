using Godot;
using System;

[GlobalClass]
public partial class Duplicator : Entity
{

	[Export]
	public AudioStreamPlayer DuplicateSound { get; set; }
	[Export]
	public Sprite2D Sprite { get; set; }
	[Export]
	public float TurnSpeed { get; set; } = 2.5f;
	[Export(PropertyHint.File)]
	public string DuplicateString { get; set; }

	public static uint Count { get; set; } = 0;
	public Vector2 PlayerDir { get; set; }
	public Vector2 Direction { get; set; }
	public float CurrentSpeed { get; set; }

	public override void _EnterTree()
	{
		base._EnterTree();
		Count++;
	}
	public override void _ExitTree()
	{
		base._EnterTree();
		Count--;
	}
	public override void Move(double _delta)
	{
		Velocity = Direction * CurrentSpeed;
		Sprite.Rotation = Direction.X * Mathf.Pi / 6f;

		MoveAndSlide();
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

	public void UpdateDir()
	{

		PlayerDir = GlobalPosition.DirectionTo(Global.Player.GlobalPosition);
	}

	public void DuplicateSpawn()
	{
		if (Count > 10) return;
		if (Health.CurrentHealth < 2) return;
		if (Global.Player.Dead) return;
		var scene = GD.Load<PackedScene>(DuplicateString);
		var dup = scene.Instantiate<Duplicator>();
		dup.Transform = GlobalTransform;
		LevelHandler.Single.SpawnEnemy(dup);
		dup.CurrentSpeed = 50f;
		dup.Direction = Vector2.Right.Rotated((float)GD.RandRange(0f, Mathf.Tau));
		dup.Health.CurrentHealth = Health.CurrentHealth - 1;
		DuplicateSound.Play();
	}
}
