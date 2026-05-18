using Godot;
using System;

[GlobalClass]
public partial class Entity : CharacterBody2D
{
	[Export]
	public float MoveSpeed { get; set; }

	[Export]
	public float Acceleration { get; set; }

	[Export]
	public float Friction { get; set; }

	[Export]
	public StateMachine StateMachine { get; set; }
	public override void _Ready()
	{
	}

	public void Move(double delta)
	{
		var col = MoveAndCollide((float)delta * Velocity);
		if (col is not null)
		{
			var norm = col.GetNormal();
			Velocity = Velocity.Slide(norm).Normalized() * Velocity.Length();
		}
		else
		{
			var global = GetNode<Global>("/root/Global");
			var PlayArea = global.World.PlayArea;
			var translatedPos = Position;
			var x = translatedPos.X;
			var y = translatedPos.Y;

			var halfX = PlayArea.Size.X / 2f;
			var halfY = PlayArea.Size.Y / 2f;
			if (x <= -halfX)
			{
				x = -halfX;
				Velocity = Velocity.Slide(Vector2.Right).Normalized() * Velocity.Length();
			}
			else if (x >= halfX)
			{
				x = halfX;
				Velocity = Velocity.Slide(Vector2.Left).Normalized() * Velocity.Length();
			}


			if (y <= -halfY)
			{
				y = -halfY;
				Velocity = Velocity.Slide(Vector2.Down).Normalized() * Velocity.Length();
			}
			else if (y >= halfY)
			{
				y = halfY;
				Velocity = Velocity.Slide(Vector2.Up).Normalized() * Velocity.Length();
			}
			Position = new(x, y);
		}
	}
}
