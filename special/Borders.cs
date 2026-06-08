using Godot;
using System;

[GlobalClass]
[Tool]
public partial class Borders : Node2D
{

	[Export]
	private World2d World { get; set; }

	public override void _Ready()
	{
		base._Ready();
		World.Connect("Redraw", Callable.From(QueueRedraw));
	}
	public override void _Draw()
	{
		base._Draw();

		Rect2 r = new(-World.Size / 2f, World.Size);
		Rect2 outerRect = new(-(Vector2)GetWindow().Size / 2f + new Vector2(0.5f, 0.5f), (Vector2)GetWindow().Size - new Vector2(0.5f, 0.5f));
		DrawRect(r, World.Color, filled: false, width: 1f);
		DrawRect(outerRect, World.Color, filled: false, width: 1f);
	}
}
