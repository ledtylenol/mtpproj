using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class RunsHistory : Resource
{
	[Export]
	public Array<Run> Runs { get; set; } = [];


	public void Save()
	{
		ResourceSaver.Save(this, "user://runs.tres");
	}
	public static RunsHistory Load()
	{
		if (!ResourceLoader.Exists("user://runs.tres")) return null;

		return ResourceLoader.Load<RunsHistory>("user://runs.tres");
	}

	public void AddRun(Run run)
	{
		Runs.Add(run);
	}
}
