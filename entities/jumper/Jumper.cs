using Godot;
using System;

public partial class Jumper : Entity
{
	[Export]
	public Sprite2D Sprite { get; set; }

	[Export]
	public float JumpHeight { get; set; }
	[Export]
	public Curve JumpCurve { get; set; }

	[Export]
	public Timer JumpTimer { get; set; }
	[Export]
	public float Cooldown { get; set; } = 0.5f;

	[Export]
	public float JumpTime { get; set; } = 0.5f;
	[Export]
	public float MaxJumpDistance { get; set; } = 75f;

	[Export]
	public HitBox HitBox { get; set; }

	[Export]
	public HurtBox HurtBox { get; set; }
	[Export]
	private float X { get; set; } = 0f;

	public Tween Tween { get; set; }

	public override void _Ready()
	{
		base._Ready();
		JumpTimer.Start(GD.RandRange(0.5, 1.8));

	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Sprite.Position = Sprite.Position with { Y = -JumpCurve.SampleBaked(X) * JumpHeight };
	}

	public void Jump(Vector2 pos)
	{
		pos = pos.LimitLength(MaxJumpDistance);
		GD.Print("JUMPED");
		Tween?.Kill();
		Tween = CreateTween();
		HitBox.Active = false;

		var sign = Mathf.Sign(pos.X);
		Tween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic).TweenProperty(this, "X", 1f, JumpTime / 2f);
		Tween.Parallel().TweenProperty(Sprite, "rotation", sign * Mathf.Pi, JumpTime / 2f).From(0f);
		Tween.SetEase(Tween.EaseType.In).TweenProperty(this, "X", 0f, JumpTime / 2f);
		Tween.Parallel().TweenProperty(Sprite, "rotation", sign * Mathf.Tau, JumpTime / 2f);
		Velocity = pos;
	}

	public void MoveBounce(double _delta)
	{
		var iterations = 5;
		var subdelt = _delta / iterations;

		for (int i = 0; i < iterations; i++)
		{
			var res = MoveAndCollide(Velocity * (float)subdelt);

			if (res is not null)
			{
				Velocity = Velocity.Bounce(res.GetNormal());
			}
		}
	}
}
