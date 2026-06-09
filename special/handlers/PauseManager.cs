using Godot;
using System;

[GlobalClass]
public partial class PauseManager : Node
{
	public static PauseManager Single { get; set; }

	public override void _Ready()
	{
		base._Ready();
		Single = this;
		ProcessMode = ProcessModeEnum.Always;
	}
	public void TogglePause()
	{
		GetTree().Paused = !GetTree().Paused;
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (Input.IsActionJustPressed("pause")) TogglePause();
	}

	public void Pause() => GetTree().Paused = true;
	public void Resume() => GetTree().Paused = false;

}
