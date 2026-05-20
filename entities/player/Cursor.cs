using Godot;
using System;

[GlobalClass]
public partial class Cursor : Node2D
{
	[Export]
	private Player Player { get; set; }

	[Export]
	public float Width { get; set; }

	[Export]
	public float NormRadius { get; set; }

	[Export]
	public float LineLength { get; set; }

	[Export]
	public float Dash { get; set; }

	[Export]
	public Color Color { get; set; }

	[Export]
	public float ShiftSpeed { get; set; }

	[Export]
	public float ShiftPower { get; set; }


	private Vector2 _MouseDir;
	public Vector2 MouseDir
	{
		get => _MouseDir; set
		{
			_MouseDir = value;
			QueueRedraw();
		}
	}

	public Vector2 MousePos { get; set; }

	public float HueShift { get; set; }
	public override void _Ready()
	{
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		MousePos = GetGlobalMousePosition();

		MouseDir = Player.Position.DirectionTo(MousePos);
		HueShift += (float)delta * ShiftSpeed;
	}

	public override void _Draw()
	{
		base._Draw();
		var norm = MouseDir.Normalized() * NormRadius;
		var color = Color;
		var hueShift = (Mathf.Sin(HueShift) + 1f) * 0.5f;
		color.OkHslH = hueShift * ShiftPower + Color.OkHslH;

		DrawDashedLine(norm, norm + MouseDir.Normalized() * LineLength, color, aligned: false, dash: Dash, width: Width);

	}
}
