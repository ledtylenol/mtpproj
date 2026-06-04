using Godot;
using System;

[GlobalClass]
public partial class RectLine : Node2D
{

	private Vector2 _Radius;
	[Export]
	private Vector2 Radius
	{
		get => _Radius; set
		{
			_Radius = value;
			QueueRedraw();
		}
	}

	public override void _Draw()
	{
		base._Draw();
		DrawRect(new(-Radius / 2f, Radius), GetNode<Global>("/root/Global").World.Color, false, width: 1f);
	}
}
