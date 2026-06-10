using Godot;
using System;

[GlobalClass]
public partial class PlayerCamera : Camera2D
{
	[Export]
	public Curve TweenIntensityCurve { get; set; }

	private Tween Tween { get; set; }
	private double timeSinceLastTween = 0f;
	public override void _Ready()
	{
		base._Ready();
		LevelHandler.Single.LevelFinished += TweenZoom;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		timeSinceLastTween += delta;
	}
	private void TweenZoom()
	{
		Tween?.Kill();
		Tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
		Tween.TweenProperty(this, "zoom", Vector2.One, 1.0).From(Vector2.One * TweenIntensityCurve.SampleBaked((float)timeSinceLastTween));
		timeSinceLastTween = 0f;
		ResetPhysicsInterpolation();
	}
}
