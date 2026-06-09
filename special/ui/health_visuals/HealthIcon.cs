using Godot;
using System;

[GlobalClass]
public partial class HealthIcon : MarginContainer
{
	[Export]
	public Node Shaker { get; set; }
	[Export]
	public bool Filled { get; set; } = true;

	[Export]
	public TextureRect Icon { get; set; }

	[Export]
	public Texture2D ShownTexture { get; set; }

	[Export]
	public Texture2D HideTexture { get; set; }

	private Tween Tween { get; set; }

	public Tween HideIcon()
	{
		Tween?.Kill();
		Tween = CreateTween();
		Tween.TweenCallback(Callable.From(() => Icon.Texture = HideTexture));
		Tween.TweenCallback(Callable.From(() => Filled = false));
		Tween.TweenCallback(Callable.From(() => Shaker.Call("play_shake")));
		return Tween;
	}

	public Tween ShowIcon()
	{
		Tween?.Kill();
		Tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
		Tween.TweenCallback(Callable.From(() => Icon.Texture = ShownTexture));
		Tween.TweenProperty(Icon, "offset_transform_scale", Vector2.One, 1f).From(Vector2.Zero);
		Tween.TweenCallback(Callable.From(() => Filled = true));
		return Tween;
	}
}
