using Godot;
using Godot.Collections;
using System;
using System.Linq;

[GlobalClass]
public partial class BgHandler : Node
{
	private Tween Tween { get; set; }
	private Tween DistanceTween { get; set; }
	private Tween DirTween { get; set; }
	[Export]
	public Array<BgShader> Shaders { get; set; } = [];


	private float Heat { get; set; }

	public override void _Ready()
	{
		base._Ready();
		LevelHandler.Single.BossNextTurn += () => TweenTimeScale(0.001f, 1f);
		LevelHandler.Single.BossNextTurn += () => TweenDir(Vector2.Up, 1f);

		Global.Player.Health.Died += (e) => TweenTimeScale(0f, 5f);
		LevelHandler.Single.BossEnemyKilled += () =>
		{
			Heat += 0.2f;
			TweenTimeScale(Heat, 1f);

		};

		LevelHandler.Single.LevelFinished += () => Heat = 0f;
		LevelHandler.Single.LevelFinished += () => TweenTimeScale(0.05f, 0.5f);
		LevelHandler.Single.LevelFinished += () => TweenDir(Vector2.Down, 1f);

		LevelHandler.Single.LevelStarted += () => { if (!LevelHandler.Single.IsBoss()) TweenTimeScale(0.2f, 0.5f); };
		PauseManager.Single.Paused += TweenPrimaryBg;
		PauseManager.Single.UnPaused += UnTweenPrimaryBg;
	}
	public void TweenTimeScale(float newScale, float duration)
	{
		Tween?.Kill();

		Tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic).SetParallel();
		foreach (var shader in Shaders)
			Tween.TweenProperty(shader, "TimeScale", newScale, duration);

	}
	public void TweenTimeScaleFrom(float newScale, float duration, float add)
	{
		Tween?.Kill();

		Tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic).SetParallel();
		foreach (var shader in Shaders)
			Tween.TweenProperty(shader, "TimeScale", newScale, duration).From(add);

	}

	public void TweenDir(Vector2 newDir, float duration)
	{
		DirTween?.Kill();

		DirTween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic).SetParallel();
		foreach (var shader in Shaders)
			DirTween.TweenProperty(shader, "TargetDir", newDir, duration);

	}
	private void UnTweenPrimaryBg()
	{
		DistanceTween?.Kill();
		DistanceTween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);

		var primaryBg = Shaders.First();

		DistanceTween.TweenProperty(primaryBg, "DistanceThreshold", 1f, 1f);
	}

	private void TweenPrimaryBg()
	{
		DistanceTween?.Kill();
		DistanceTween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Expo);

		var primaryBg = Shaders.First();

		DistanceTween.TweenProperty(primaryBg, "DistanceThreshold", 10f, 10f);
	}
}
