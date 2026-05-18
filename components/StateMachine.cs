using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class StateMachine : Node
{
	[Export] private State InitialState { get; set; }
	private State CurrentState { get; set; }
	private Dictionary<string, State> States { get; set; } = [];

	public override void _Ready()
	{
		base._Ready();
		// manually control flow of state machine instead of relying on the scene tree
		SetProcess(false);
		SetPhysicsProcess(false);
		foreach (var child in GetChildren())
		{
			if (child is State state)
			{
				States.Add(state.Name.ToString().ToLower(), state);
				state.Transitioned += Transition;
			}
		}
		InitialState.OnEnter();
	}

	public void Tick(double delta)
	{
		CurrentState.Tick(delta);
	}

	public void PhysicsTick(double delta)
	{
		CurrentState.PhysicsTick(delta);
	}

	//handle transition logic from one state to another
	private void Transition(string to)
	{
		var lower = to.ToLower();
		var found = States.TryGetValue(lower, out State newState);

		if (!found)
		{
			GD.PushError($"could not find {lower}");
			return;
		}

		CurrentState.OnExit();

		CurrentState = newState;

		CurrentState.OnEnter();
	}

}
