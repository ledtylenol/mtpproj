using Godot;
using System;

[GlobalClass]
public partial class Point : Entity
{
	[Export]
	public float Points { get; set; }
	public static uint LastIndex { get; set; } = 0;

	public override void _Ready()
	{
		base._Ready();
		Scale = Vector2.One * (1f + Points / 2000f);
	}

	public override void _Draw()
	{
		base._Draw();
		DrawRect(new(Vector2.Zero, Vector2.One * 3f), Colors.White);
	}
	public override void Move(double _delta)
	{
		Position += Velocity * (float)_delta;
	}

}
