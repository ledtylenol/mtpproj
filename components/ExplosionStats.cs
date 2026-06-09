using Godot;
using System;

[GlobalClass]
public partial class ExplosionStats : Resource
{
	[Export]
	private Curve InnerWidthCurve { get; set; }
	[Export]
	private Curve OuterWidthCurve { get; set; }
	[Export]
	private Tween.EaseType InitEaseType { get; set; }
	[Export]
	private Tween.TransitionType InitTransType { get; set; }
	[Export]
	private Tween.EaseType EaseType { get; set; }
	[Export]
	private Tween.TransitionType TransType { get; set; }
	[Export]
	public float EndRadius { get; private set; }

	[Export]
	public float InitDuration { get; set; }
	[Export]
	public float Duration { get; set; }

	public const float ActiveWidthThreshold = 0.9f;
	public const uint MaskAll = 1 << 1 | 1 << 2;
	public const uint LayerAll = 1 << 1 | 1 << 2;

	public const uint MaskEnemy = 1 << 2;
	public const uint LayerEnemy = 1 << 1;

	public const uint MaskPlayer = 1 << 1;
	public const uint LayerPlayer = 1 << 2;

	[Export]
	public int Damage { get; set; }

	public float Radius(float Ratio) => Mathf.Lerp(0f, EndRadius, Ratio);
	public float InnerWidth(float Ratio) => InnerWidthCurve.SampleBaked(Ratio);
	public float OuterWidth(float Ratio) => OuterWidthCurve.SampleBaked(Ratio);
	public Tween GetAndStartTween(Explosion expl)
	{
		var tween = expl.CreateTween().SetEase(InitEaseType).SetTrans(InitTransType);

		if (InitDuration > 0f)
			tween.TweenProperty(expl, "InitRatio", 1f, InitDuration);
		tween.TweenCallback(Callable.From(expl.Activate));
		tween.SetEase(EaseType).SetTrans(TransType).TweenProperty(expl, "Ratio", 1f, Duration);

		return tween;
	}
	public Tween GetTween(Explosion expl) => expl.CreateTween().SetEase(EaseType).SetTrans(TransType);
}
