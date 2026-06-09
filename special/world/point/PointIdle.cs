using Godot;
using System;

[GlobalClass]
public partial class PointIdle : State
{
	public override void _Ready()
	{
		base._Ready();
		LevelHandler.Single.Connect("LevelFinished", Callable.From(OnLevelChange), 4);
	}
	public override void OnEnter()
	{
	}

	public override void OnExit()
	{
	}

	public override void PhysicsTick(double delta)
	{
		Entity.Velocity = Entity.Velocity.MoveToward(Vector2.Zero, Entity.Friction * (float)delta);
		Entity.Move(delta);
	}

	public override void Tick(double delta)
	{
	}

	private void OnLevelChange()
	{
		EmitSignalTransitioned("Trace");

	}

}
