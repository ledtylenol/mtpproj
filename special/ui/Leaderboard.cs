using Godot;
using System;

[GlobalClass]
public partial class Leaderboard : MarginContainer
{
	[Export]
	public VBoxContainer Container { get; set; }

	[Export]
	public PackedScene LeaderboardEntry { get; set; }
	public override void _Ready()
	{
		base._Ready();
		Global.Single.RunEnded += AddRun;
		var runs = Global.Runs.Runs.Duplicate();
		runs.Reverse();
		foreach (var run in runs)
		{
			var entry = LeaderboardEntry.Instantiate<LeaderboardEntry>();

			Container.AddChild(entry);
			entry.Initialize(run);
		}
	}
	public void AddRun(Run run)
	{
		var entry = LeaderboardEntry.Instantiate<LeaderboardEntry>();

		Container.AddChild(entry);
		Container.MoveChild(entry, 0);
		entry.Initialize(run);

	}
}
