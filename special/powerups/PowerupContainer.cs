using Godot;
using System;
using static Godot.Mathf;

[GlobalClass]
public partial class PowerupContainer : Node2D
{
	[Export]
	public Powerup Powerup { get; set; }

	[Export]
	public Sprite2D Sprite { get; set; }
	[Export]
	public AudioStreamPlayer Sound { get; set; }
	public override void _Ready()
	{
		base._Ready();
		Powerup.PowerupApplied += SpawnVisuals;
		Powerup.Active = false;
		Powerup.ActivateAfter(0.5f);
	}

	private void SpawnVisuals(string message)
	{
		Sprite.Hide();
		var label = new Label
		{
			Text = message,
			Position = GlobalPosition,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,

		};
		Global.Single.ForceSpawn2D(label);

		var tween = label.CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
		var th = (float)GD.RandRange(-Pi / 2, Pi / 2);
		var r = (float)GD.RandRange(20f, 80f);

		var dir = Vector2.Up.Rotated(th) * r;

		var copy = Sound.Duplicate() as AudioStreamPlayer;
		Global.Single.Spawn(copy);
		copy.Play();

		copy.Finished += copy.QueueFree;

		tween.TweenProperty(label, "position", dir, 1f).AsRelative();
		tween.TweenCallback(Callable.From(label.QueueFree));
		QueueFree();
	}
}
