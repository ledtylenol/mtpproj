using Godot;
using System;

public partial class Global : Node
{
	//Singleton for convenience
	public static Global Single { get; private set; }
	public static World2d World { get; set; }
	public static Player Player { get; set; }

	[Signal]
	public delegate void Spawn2DEventHandler(Node2D sc);

	[Signal]
	public delegate void SpawnOtherWorldEventHandler(Node2D sc);
	[Signal]
	public delegate void SpawnUIEventHandler(Control sc);

	[Signal]
	public delegate void LevelChangedEventHandler();
	[Signal]
	public delegate void EntityDiedEventHandler(Entity e, Entity culprit);

	public override void _EnterTree()
	{
		base._EnterTree();
		Single = this;
	}
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Keycode == Key.R)
		{
			EmitSignalLevelChanged();
		}

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
	public void SpawnOther(Node2D src)
	{
		EmitSignalSpawnOtherWorld(src);
	}

}
