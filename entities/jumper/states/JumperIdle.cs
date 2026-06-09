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
		var tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
		tween.TweenProperty(Sprite, "scale", new Vector2(1.2f, 0.8f), JumpDuration).From(Vector2.One);
		tween.TweenCallback(Callable.From(() => JumpTimer.Start(Jumper.Cooldown + GD.RandRange(0.5, 0.9))));
		tween.TweenCallback(Callable.From(() => EmitSignalTransitioned("jump")));
		tween.SetTrans(Tween.TransitionType.Elastic);
		tween.TweenProperty(Sprite, "scale", new Vector2(0.7f, 1.3f), 0.75);
		tween.TweenProperty(Sprite, "scale", Vector2.One, 0.5);
	}

}
