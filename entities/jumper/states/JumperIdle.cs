using Godot;
using System;

[GlobalClass]
public partial class JumperIdle : JumperState
{
	[Export]
	public Timer JumpTimer { get; set; }
	[Export]
	public Sprite2D Sprite { get; set; }

	[Export]
	public float JumpDuration { get; set; }

	private Tween Tween { get; set; }
	public override void OnEnter()
	{
		JumpTimer.Timeout += Jump;
	}

	public override void OnExit()
	{
		JumpTimer.Timeout -= Jump;
	}

	public override void PhysicsTick(double delta)
	{
		Jumper.Velocity = Jumper.Velocity.MoveToward(Vector2.Zero, (float)delta * Jumper.Friction);
		Jumper.Move(delta);
	}

	public override void Tick(double delta)
	{
	}

	private void Jump()
	{
		Tween?.Kill();
		Tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
		Tween.TweenProperty(Sprite, "scale", new Vector2(1.2f, 0.8f), JumpDuration).From(Vector2.One);
		Tween.TweenCallback(Callable.From(() => JumpTimer.Start(Math.Max(Jumper.Cooldown, Jumper.JumpTime) + GD.RandRange(0.5, 0.9))));
		Tween.TweenCallback(Callable.From(() => EmitSignalTransitioned("jump")));
		Tween.SetTrans(Tween.TransitionType.Elastic);

		Tween.TweenProperty(Sprite, "scale", new Vector2(0.7f, 1.3f), JumpDuration / 2f);
		Tween.TweenProperty(Sprite, "scale", Vector2.One, JumpDuration / 2f);
	}

}
