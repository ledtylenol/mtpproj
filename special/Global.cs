using Godot;
using System;

public partial class Global : Node
{
	//Singleton for convenience
	public static Global Single { get; private set; }
	public static World2d World { get; set; }
	public static Player Player { get; set; }
	public float Score { get; set; } = 0f;
	[Signal]
	public delegate void Spawn2DEventHandler(Node sc);

	[Signal]
	public delegate void SpawnOtherWorldEventHandler(Node2D sc);
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


}
