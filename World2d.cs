using Godot;
using System;

[Tool]
[GlobalClass]
public partial class World2d : Node2D
{

	private Vector2 _Size;
	[Export]
	public Vector2 Size
	{
		get => _Size;
		set
		{
			_Size = value;
			UpdatePlayArea();
			QueueRedraw();
		}
	}

	public Rect2 PlayArea { get; set; }

	private Color _Color;
	[Export]
	public Color Color
	{
		get => _Color;
		set
		{
			_Color = value;
			QueueRedraw();
		}
	}

	public override void _Ready()
	{
		if (Engine.IsEditorHint()) return;

		var global = GetNode<Global>("/root/Global");
		global.World = this;
		UpdatePlayArea();
	}

	public void UpdatePlayArea()
	{
		PlayArea = new(-Size / 2f, Size);
		GD.Print($"Play area updated: {PlayArea}");
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}
	public override void _Draw()
	{
		base._Draw();
		GD.Print($"DARAW RECT OF SIZE {Size} WITH COLOR {Color}");

		Rect2 r = new(-Size / 2f, Size);
		DrawRect(r, Color, filled: false, width: 1f);
	}

}
