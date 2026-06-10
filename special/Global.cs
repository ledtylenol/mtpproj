using Godot;
using System;

public partial class Global : Node
{
	//Singleton for convenience
	public static Global Single { get; private set; }
	public static World2d World { get; set; }
	public static Player Player { get; set; }
	public static Settings Settings { get; set; }
	public static Run Run { get; set; }
	public static RunsHistory Runs { get; set; }
	[Signal]
	public delegate void Spawn2DEventHandler(Node sc);

	[Signal]
	public delegate void SpawnOtherWorldEventHandler(Node2D sc);
	[Signal]
	public delegate void SpawnUIEventHandler(Control sc);

	[Signal]
	public delegate void EntityDiedEventHandler(Entity e, Entity culprit);

	[Signal]
	public delegate void RunEndedEventHandler(Run run);

	public bool Counting { get; set; } = false;

	public override void _EnterTree()
	{
		base._EnterTree();
		Single = this;
		Runs = RunsHistory.Load();
		Runs ??= new();
		foreach (var run in Runs.Runs)
		{
			GD.Print($"run with {run.Score} score lasted {run.Time}s");
		}

		Settings = Settings.Load();
		Settings ??= new();
		Run = new Run();
	}
	public override void _Ready()
	{
		base._Ready();

		CallDeferred("ConnectHandlers");
	}

	private void ConnectHandlers()
	{
		LevelHandler.Single.LevelFinished += StopCounting;
		LevelHandler.Single.LevelStarted += StartCounting;
		Player.Health.Died += (e) => SubmitRun();
	}
	public void Spawn(Node2D src)
	{
		EmitSignalSpawn2D(src);
	}
	public void Spawn(Node src)
	{
		AddChild(src);
	}
	public void Spawn(Control src)
	{
		EmitSignalSpawnUI(src);
	}

	public void ForceSpawn2D(Node src)
	{
		EmitSignalSpawn2D(src);
	}
	public void SpawnOther(Node2D src)
	{
		EmitSignalSpawnOtherWorld(src);
	}


	public override void _Process(double delta)
	{
		base._Process(delta);
		if (Counting)
			Run.Time += (float)delta;
	}
	public void StopCounting()
	{
		Counting = false;
	}

	public void StartCounting()
	{
		Counting = true;
	}

	public static void SubmitRun()
	{
		Runs.AddRun(Run);
		Runs.Save();
		Single.EmitSignalRunEnded(Run);
		Run = new();
	}
}
