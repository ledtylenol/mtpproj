using Godot;
using System;

[GlobalClass]
public partial class DuplicatorIdle : State
{
	[Export]
	public Duplicator Duplicator { get; set; }
	private SceneTreeTimer Timer { get; set; }
	public override void OnEnter()
	{
		Timer = GetTree().CreateTimer(0.5f, false);
		Timer.Timeout += () => EmitSignalTransitioned("follow");
	}

	public override void OnExit()
	{
	}

	public override void PhysicsTick(double delta)
	{
		float weight = -(float)delta * Duplicator.Friction;
		Duplicator.CurrentSpeed = Mathf.Lerp(Duplicator.CurrentSpeed, 0f, 1.0f - Mathf.Exp(weight));
		Duplicator.Move(delta);
	}

	public override void Tick(double delta)
	{
	}

}
