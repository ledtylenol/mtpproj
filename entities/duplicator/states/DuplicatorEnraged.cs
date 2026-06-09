using Godot;
using System;

[GlobalClass]
public partial class DuplicatorEnraged : State
{

	[Export]
	public Duplicator Duplicator { get; set; }

	[Export]
	public Ghosts Ghosts { get; set; }

	[Export]
	public Sprite2D Sprite { get; set; }

	[Export]
	public Node2D Shaker { get; set; }

	[Export]
	public AudioStreamPlayer Windup { get; set; }

	[Export]
	public AudioStreamPlayer Dash { get; set; }
	private Tween Tween { get; set; }
	public override void OnEnter()
	{
		Tween?.Kill();
		Tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
		var playerPos = Global.Player.GlobalPosition;
		Tween.TweenCallback(Callable.From(() => Windup.Play()));
		Tween.TweenProperty(Sprite, "scale", new Vector2(1.2f, 0.8f), 0.5);
		Tween.Parallel().TweenProperty(Shaker, "intensity", 1f, 0.5);
		Tween.Parallel().TweenProperty(Duplicator, "CurrentSpeed", 0f, 0.5);
		Tween.TweenCallback(Callable.From(() =>
		{
			Dash.Play();
			Duplicator.CurrentSpeed = 400f;
			Duplicator.Direction = GetDir();
			Ghosts.Active = true;
			Sprite.Scale = Vector2.One;
		}));
		Tween.TweenProperty(Shaker, "intensity", 0f, 0.5);
		Tween.TweenCallback(Callable.From(() =>
		{
			EmitSignalTransitioned("idle");
			Ghosts.Active = false;
		}));
	}

	public override void OnExit()
	{
		Ghosts.Active = false;
	}


	public Vector2 GetDir()
	{

		var playerPos = Global.Player.GlobalPosition;
		var dist = Duplicator.GlobalPosition.DistanceTo(playerPos) / Duplicator.CurrentSpeed;
		return Duplicator.GlobalPosition.DirectionTo(playerPos + Global.Player.Velocity * dist);
	}

	public override void PhysicsTick(double delta)
	{
		float weight = -(float)delta * Duplicator.TurnSpeed / 5f;
		Duplicator.UpdateDir();
		Duplicator.Direction = Duplicator.Direction.Slerp(Duplicator.PlayerDir, 1.0f - Mathf.Exp(weight));
		Duplicator.Move(delta);
	}

	public override void Tick(double delta)
	{
	}

}
