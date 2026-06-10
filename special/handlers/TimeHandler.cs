using Godot;
using System;

public partial class TimeHandler : Node
{
	public static TimeHandler Single { get; set; }
	private static Tween Tween { get; set; }
	public static void Stop(float time, float delay)
	{
		Tween?.Kill();
		Tween = Single.CreateTween().SetIgnoreTimeScale().SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Expo);
		Engine.TimeScale = 0f;
		Tween.TweenInterval(delay);
		Tween.TweenProperty(Engine.Singleton, "time_scale", 1f, time);
	}

	public override void _Ready()
	{
		base._Ready();
		Single = this;
	}

}
