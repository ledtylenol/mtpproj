using Godot;
using System;

[GlobalClass]
public partial class PauseManager : Node
{
	public static PauseManager Single { get; set; }

	[Signal]
	public delegate void PausedEventHandler();

	[Signal]
	public delegate void UnPausedEventHandler();
	public override void _Ready()
	{
		base._Ready();
		Single = this;
		ProcessMode = ProcessModeEnum.Always;
	}
	public static void TogglePause()
	{
		var wasPaused = Single.GetTree().Paused;
		Single.GetTree().Paused = !Single.GetTree().Paused;
		if (!wasPaused)
			Single.EmitSignalPaused();
		else
			Single.EmitSignalUnPaused();
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (Input.IsActionJustPressed("pause")) TogglePause();
	}

	public static void Pause()
	{
		Single.GetTree().Paused = true;
		Single.EmitSignalPaused();
	}
	public static void Resume()
	{
		Single.GetTree().Paused = false;
		Single.EmitSignalUnPaused();
	}

}
