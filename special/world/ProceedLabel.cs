using Godot;
using System;

[GlobalClass]
public partial class ProceedLabel : RichTextLabel
{

	private float _Spacing;
	[Export]
	public float Spacing
	{
		get => _Spacing; set
		{
			_Spacing = value;
			Text = $"[font gl={Spacing}][font_size=40][center]{Message}";
		}
	}

	private string Message { get; set; }

	[Export]
	public float InitialSpacing { get; set; }
	private Tween Tween { get; set; }
	public override void _Ready()
	{
		base._Ready();
		LevelHandler.Single.LevelStarted += Hide;
		LevelHandler.Single.LevelFinished += ShowFinished;
		Global.Player.Died += ShowDead;
	}

	private void ShowFinished()
	{
		Tween?.Kill();
		Tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
		Tween.TweenProperty(this, "Spacing", 0f, 2f).From(InitialSpacing);
		Show();
		Message = "[TAB]\nPROCEED";
	}

	private void ShowDead()
	{
		Tween?.Kill();
		Tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
		Tween.TweenProperty(this, "Spacing", 0f, 2f).From(InitialSpacing);
		Show();
		Message = "YOU DIED\n[TAB]\nPROCEED";
	}
}
