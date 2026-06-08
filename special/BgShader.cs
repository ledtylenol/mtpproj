using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
[Tool]
public partial class BgShader : ColorRect
{
	[Export]
	public World2d World { get; set; }

	[Export]
	public float TimeScale { get; set; }

	[Export]
	public float TweenDuration { get; set; }

	[Export]
	public Tween.EaseType EaseType { get; set; }

	[Export]
	public Tween.TransitionType TransType { get; set; }

	private float _Time;
	private float Time
	{
		get => _Time; set
		{
			_Time = value;
			SetInstanceShaderParameter("time", value);
		}
	}

	private Vector2 _FlowDir;

	public Vector2 FlowDir
	{
		get => _FlowDir; set
		{
			_FlowDir = value;
			SetInstanceShaderParameter("flow_dir", value);
		}
	}

	private Vector2 TargetDir;
	private float TargetTimeScale;

	private float _DistanceThreshold;
	public float DistanceThreshold
	{
		get => _DistanceThreshold; set
		{
			_DistanceThreshold = value;
			SetInstanceShaderParameter("distance_threshold", value);
		}
	}

	private Tween Tween { get; set; }
	private Tween DistTween { get; set; }
	public override void _Ready()
	{
		base._Ready();
		World.PlayAreaUpdated += UpdateSize;
		UpdateSize(World.PlayArea);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Time += (float)delta * TimeScale;
		if (Engine.IsEditorHint()) return;
		FlowDir += TargetDir * (float)delta * TimeScale;
		if (Input.IsActionJustPressed("light")) { TweenDistanceThreshold(1f); TweenTimeScale(0.2f, Vector2.Down); }
	}
	private void UpdateSize(Rect2 playArea)
	{
		Size = playArea.Size;
		Position = -Size / 2;
	}

	public void TweenTimeScale(float newTimeScale, Vector2 newFlowDir)
	{
		if (TargetDir == newFlowDir) return;
		Tween?.Kill();
		Tween = CreateTween().SetEase(EaseType).SetTrans(TransType).SetParallel();

		Tween.TweenProperty(this, "TimeScale", newTimeScale, TweenDuration);
		Tween.TweenProperty(this, "TargetDir", newFlowDir, TweenDuration);
	}

	public void TweenDistanceThreshold(float newDistanceThreshold)
	{
		DistTween?.Kill();
		DistTween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo).SetParallel();

		DistTween.TweenProperty(this, "DistanceThreshold", newDistanceThreshold, TweenDuration / 2f);
	}
}
