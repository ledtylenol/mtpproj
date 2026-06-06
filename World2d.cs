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

	[Export]
	public Label Label { get; set; }

	[Export]
	public Health PlayerHealth { get; set; }

	[Signal]
	public delegate void PlayerDiedEventHandler(Player Player);

	[Signal]
	public delegate void PlayAreaUpdatedEventHandler(Rect2 newArea);
	private int x = 0;
	public override void _Ready()
	{
		if (Engine.IsEditorHint()) return;
		Texture = new();
		ClearTexture();

		var global = Global.Single;
		global.World = this;
		UpdatePlayArea();
		if (PlayerHealth is not null)
			PlayerHealth.Died += (e) => EmitSignalPlayerDied((Player)e);
		Global.Single.Spawn2D += SpawnEntity;
	}

	public void ClearTexture()
	{
		var size = PlayArea.Size;
		Texture.Setup((int)size.X, (int)size.Y, DrawableTexture2D.DrawableFormat.Rgba8, Godot.Colors.Black);
	}
	public void UpdatePlayArea()
	{
		PlayArea = new(-Size / 2f, Size);
		EmitSignalPlayAreaUpdated(PlayArea);
	}
	public override void _PhysicsProcess(double delta)
	{
		if (Engine.IsEditorHint()) return;
		base._PhysicsProcess(delta);
		x++;
		Label.Text = $"{x:D9}";

	}
	public override void _Draw()
	{
		base._Draw();

		Rect2 r = new(-Size / 2f, Size);
		Rect2 outerRect = new(-(Vector2)GetWindow().Size / 2f + new Vector2(0.5f, 0.5f), (Vector2)GetWindow().Size - new Vector2(0.5f, 0.5f));
		DrawRect(r, Color, filled: false, width: 1f);
		DrawRect(outerRect, Color, filled: false, width: 1f);
	}

	private void SpawnEntity(Node2D e)
	{
		AddChild(e);
		GD.Print(e);
	}
}
