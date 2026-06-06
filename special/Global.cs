using Godot;
using System;

public partial class Global : Node
{
	//Singleton for convenience
	public static Global Single { get; private set; }
	public World2d World { get; set; }

	[Signal]
	public delegate void Spawn2DEventHandler(Node2D sc);

	[Signal]
	public delegate void SpawnUIEventHandler(Control sc);

	[Signal]
	public delegate void EntityDiedEventHandler(Entity e, Entity culprit);

	public override void _EnterTree()
	{
		base._EnterTree();
		Single = this;
	}

	public void Spawn(Node2D src)
	{
		GD.Print("SPAWN 2D");
		EmitSignalSpawn2D(src);
	}
	public void Spawn(Control src)
	{
		EmitSignalSpawnUI(src);
	}
}
