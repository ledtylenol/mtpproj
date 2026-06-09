using Godot;
using System;

[GlobalClass]
public partial class PointTrace : State
{
	private Vector2 StartPos { get; set; }
	[Export]
	private PointApplier Applier { get; set; }
	public override void OnEnter()
	{
		StartPos = Entity.GlobalPosition;
		var tween = CreateTween().SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenInterval(GD.RandRange(0.1, 0.9));
		tween.TweenMethod(Callable.From<float>(LerpToTarget), 0.0, 1.0, 1.0);
	}

	public override void OnExit()
	{
	}

	public override void PhysicsTick(double delta)
	{
	}

	public override void Tick(double delta)
	{
	}

	private void LerpToTarget(float weight)
	{
		var playerPos = Global.Player.GlobalPosition;
		Entity.GlobalPosition = StartPos.Lerp(playerPos, weight);
		if (Entity.GlobalPosition.DistanceTo(playerPos) < 2f)
		{
			Applier.ApplyPoints();
		}
	}
}
