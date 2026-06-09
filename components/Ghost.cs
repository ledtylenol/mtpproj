using Godot;
using System;

public partial class Ghost(float duration) : Sprite2D
{
	private float Duration { get; set; } = duration;

	public override void _Ready()
	{
		base._Ready();
		ZIndex = 1;
		var tween = CreateTween();
		tween.TweenProperty(this, "modulate:a", 0f, Duration);
		tween.TweenCallback(Callable.From(QueueFree));
	}

}
