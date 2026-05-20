using Godot;
using System;

public struct Sploch
{
	public uint size;
	public char colorIndex;
	public char decayColorIndex;
	public bool hollow;

	public readonly Vector2 GetSize()
	{
		var x = size & 0xFFFF;
		var y = (size >> 16) & 0xFFFF;

		return new(x, y);
	}
	public readonly Color GetColor(Color[] colors, float decay)
	{
		return colors[colorIndex].Lerp(colors[decayColorIndex], Mathf.Min(decay, 1f));
	}

	public Sploch(Vector2 size, char i, bool hollow = false)
	{
		var x = (uint)size.X;
		var y = (uint)size.Y;

		this.size = x | y << 16;
		colorIndex = i;
		this.hollow = hollow;
	}
}
[Tool]
[GlobalClass]
public partial class World2d : Node2D
{

	public Color[] Colors = [new(0f, 0f, 0f), new(0.5f, 0.5f, 0.5f), new(1f, 1f, 1f)];
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

	private DrawableTexture2D Texture { get; set; } = new();
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

	public Sploch[] Splotches { get; set; }
	public override void _Ready()
	{
		if (Engine.IsEditorHint()) return;
		ClearTexture();

		var global = GetNode<Global>("/root/Global");
		global.World = this;
		UpdatePlayArea();
	}

	public void ClearTexture()
	{
		var size = PlayArea.Size;
		Texture.Setup((int)size.X, (int)size.Y, DrawableTexture2D.DrawableFormat.Rgba8, Godot.Colors.Black);
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
