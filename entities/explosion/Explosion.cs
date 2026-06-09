using Godot;
using System;

public partial class Explosion(ExplosionStats explosionStats, uint mask, uint layer, bool clearOnLevelFinish) : Entity
{
	[Export]
	public bool ClearOnLevelFinish { get; set; } = clearOnLevelFinish;
	[Export]
	public ExplosionStats ExplosionStats { get; set; } = explosionStats;

	public AudioStream WindUp { get; set; } = GD.Load<AudioStream>("res://entities/explosion/windup.sfxr");
	public AudioStream Boom { get; set; } = GD.Load<AudioStream>("res://entities/explosion/boom.sfxr");
	public float Ratio
	{
		get => _Ratio; set
		{
			_Ratio = value;
			QueueRedraw();
		}
	}
	private float _Ratio;

	public float InitRatio
	{
		get => _InitRatio; set
		{
			_InitRatio = value;
			QueueRedraw();
		}
	}
	private float _InitRatio;

	public bool Active { get; private set; } = false;

	private Tween Tween { get; set; }
	private HitBox HitBox { get; set; }
	private CollisionShape2D CollisionShape { get; set; }
	private CircleShape2D Shape { get; set; }
	private AudioStreamPlayer WindUpPlayer { get; set; }
	private AudioStreamPlayer BoomPlayer { get; set; }

	[Signal]
	public delegate void VisualDoneEventHandler();

	[Signal]
	public delegate void ExplosionDoneEventHandler();

	public override void _Ready()
	{
		base._Ready();
		var tween = ExplosionStats.GetAndStartTween(this);

		tween.TweenCallback(Callable.From(EmitSignalExplosionDone));
		tween.TweenCallback(Callable.From(QueueFree));
		HitBox = new()
		{
			Active = false
		};

		WindUpPlayer = new()
		{
			Stream = WindUp,
			Bus = "Sfx",

		};

		BoomPlayer = new()
		{
			Stream = Boom,
			Bus = "Sfx",
		};

		CollisionShape = new();
		Shape = new()
		{
			Radius = 0f
		};
		CollisionShape.Shape = Shape;

		HitBox.Damage = ExplosionStats.Damage;
		HitBox.CollisionMask = mask;
		HitBox.CollisionLayer = layer;

		HitBox.AddChild(CollisionShape);

		if (ClearOnLevelFinish)
			LevelHandler.Single.Connect("LevelFinished", Callable.From(QueueFree));
		AddChild(WindUpPlayer);
		if (ExplosionStats.InitDuration > 0f)
			WindUpPlayer.Play();
		AddChild(BoomPlayer);
		AddChild(HitBox);

	}

	public override void _Draw()
	{
		base._Draw();
		float radius = ExplosionStats.Radius(Ratio);
		float visualRadius = ExplosionStats.Radius(InitRatio);
		float innerWidth = ExplosionStats.InnerWidth(InitRatio);
		float visualWidth = ExplosionStats.OuterWidth(Ratio);
		float outerWidth = ExplosionStats.OuterWidth(InitRatio);
		DrawCircle(Vector2.Zero, radius, Colors.Red, filled: false, width: visualWidth);
		DrawCircle(Vector2.Zero, visualRadius, Colors.White, filled: false, width: outerWidth);
		DrawCircle(Vector2.Zero, visualRadius, Colors.Red, filled: false, width: innerWidth);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Shape.Radius = ExplosionStats.Radius(Ratio) - ExplosionStats.OuterWidth(Ratio) / 2f;
		WindUpPlayer.PitchScale = 1f - InitRatio * InitRatio / 1.2f;
		WindUpPlayer.VolumeLinear = 1f - InitRatio * InitRatio;
		if (Ratio > ExplosionStats.ActiveWidthThreshold && Active)
		{
			Active = false;
			HitBox.Active = false;
		}
	}
	public void Activate()
	{
		HitBox.Active = true;
		Active = true;
		BoomPlayer.Play();
		EmitSignalVisualDone();
	}
}
