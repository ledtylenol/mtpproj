using Godot;
using System;

[GlobalClass]
[Tool]
public partial class Borders : Node2D
{

	[Export]
	private World2d World { get; set; }

	public override void _EnterTree()
	{
		base._EnterTree();

		GetWindow().MaximizeDisabled = true;
	}
	public override void _Ready()
	{
		base._Ready();
		World.Connect("Redraw", Callable.From(QueueRedraw));
		GetWindow().SizeChanged += QueueRedraw;

		GetWindow().MaximizeDisabled = false;
	}
	public override void _Draw()
	{
		base._Draw();

		GD.Print("HERE");
		Rect2 r = new(-World.Size / 2f, World.Size);
		DrawRect(r, World.Color, filled: false, width: 1f);
		if (GetWindow().Size.Length() > 10f)
		{
			var size = (Vector2)GetWindow().Size - new Vector2(0.5f, 0.5f);
			Rect2 outerRect = new(-size / 2f, size);
			DrawRect(outerRect, World.Color, filled: false, width: 1f);
		}
	}
}
