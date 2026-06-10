using Godot;
using System;

[GlobalClass]
public partial class SettingsUi : Control
{
	[Export]
	public HSlider SfxSlider { get; set; }

	[Export]
	public Label SfxLabel { get; set; }

	[Export]
	public HBoxContainer SfxContainer { get; set; }

	[Export]
	public HSlider MusicSlider { get; set; }

	[Export]
	public Label MusicLabel { get; set; }

	[Export]
	public HBoxContainer MusicContainer { get; set; }

	private Tween Tween { get; set; }
	public override void _Ready()
	{
		base._Ready();
		SfxSlider.Value = Global.Settings.SfxVolume;
		MusicSlider.Value = Global.Settings.MusicVolume;
		SfxSlider.ValueChanged += OnSfxChanged;
		MusicSlider.ValueChanged += OnMusicChanged;

		SfxSlider.DragEnded += SaveSettings;
		MusicSlider.DragEnded += SaveSettings;


		OnSfxChanged(SfxSlider.Value);
		OnMusicChanged(MusicSlider.Value);

		PauseManager.Single.Paused += TweenMenu;
		PauseManager.Single.UnPaused += TweenNormal;

	}

	private void OnSfxChanged(double value)
	{
		Global.Settings.SetSfx((float)value);
		SfxLabel.Text = $"SFX: {value:P0}";
	}
	private void OnMusicChanged(double value)
	{
		Global.Settings.SetMusic((float)value);
		MusicLabel.Text = $"MUSIC: {value:P0}";
	}

	private void SaveSettings(bool valueChanged)
	{
		if (valueChanged) Global.Settings.Save();
	}

	public void TweenMenu()
	{
		Tween?.Kill();
		Tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Expo).SetParallel();

		var delay = 1.25f;

		var subt = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Elastic);
		subt.TweenProperty(SfxContainer, "offset_transform_position:x", -355f, 1f);

		Tween.TweenSubtween(subt).SetDelay(delay);
		delay += 0.05f;

		var subt2 = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Elastic);
		subt2.TweenProperty(MusicContainer, "offset_transform_position:x", -355f, 1f);

		Tween.TweenSubtween(subt2).SetDelay(delay);
	}

	public void TweenNormal()
	{
		Tween?.Kill();
		Tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo).SetParallel();

		var delay = 0f;

		var subt = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
		subt.TweenProperty(SfxContainer, "offset_transform_position:x", 0f, 1f);

		Tween.TweenSubtween(subt).SetDelay(delay);
		delay += 0.05f;

		var subt2 = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
		subt2.TweenProperty(MusicContainer, "offset_transform_position:x", 0f, 1f);

		Tween.TweenSubtween(subt2).SetDelay(delay);
	}

}
