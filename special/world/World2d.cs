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

	[Export]
	public Node2D SpawnSlot { get; set; }

	[Signal]
	public delegate void PlayerDiedEventHandler(Player Player);

	[Signal]
	public delegate void PlayAreaUpdatedEventHandler(Rect2 newArea);

	[Signal]
	public delegate void RedrawEventHandler();

	public override void _Ready()
	{
		if (Engine.IsEditorHint()) return;

		GD.Print("READY");
		Global.World = this;
		Global.Single.Score = 0f;

		UpdatePlayArea();
		if (PlayerHealth is not null)
			PlayerHealth.Died += (e) => EmitSignalPlayerDied((Player)e);
		Global.Single.Connect("Spawn2D", Callable.From<Node2D>(SpawnEntity));
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
		int score = (int)Global.Single.Score;
		Label.Text = $"{score:D9}";

	}

	private void SpawnEntity(Node2D e)
	{
		SpawnSlot.AddChild(e);
	}
	public void Reset()
	{
		var player = Global.Player;
		Global.Single.Score = 0f;
		EmitSignalPlayerDied(Global.Player);
		player.Health.CurrentHealth = player.Health.MaxHealth;
		player.UnDie();
		LevelHandler.Single.Reset();

		SpawnSlot.QueueFree();
		SpawnSlot = new();
		SpawnSlot.YSortEnabled = true;
		AddChild(SpawnSlot);

	}
}
