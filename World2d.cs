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

		var global = Global.Single;
		global.World = this;
		UpdatePlayArea();
		if (PlayerHealth is not null)
			PlayerHealth.Died += (e) => EmitSignalPlayerDied((Player)e);
		Global.Single.Spawn2D += SpawnEntity;
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
	}
}
