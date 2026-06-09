using Godot;
using System;

[GlobalClass]
public partial class SpawnWrapper(ExplosionStats explosionStats, Node2D nodeToSpawn) : Node2D
{
	public ExplosionStats ExplosionStats { get; set; } = explosionStats;
	public Node2D NodeToSpawn { get; set; } = nodeToSpawn;


	public override void _Ready()
	{
		base._Ready();
		var expl = new Explosion(ExplosionStats, 0, 0, false);
		expl.VisualDone += SpawnTheNode;
		expl.Position = NodeToSpawn.Position;
		Global.Single.Spawn(expl);
	}

	private void SpawnTheNode()
	{
		Global.Single.Spawn(NodeToSpawn);
		QueueFree();
	}
}
