using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Points : Node
{
	[Export]
	public uint PointCount { get; set; }
	[Export]
	public Health Health { get; set; }
	[Export]
	public PackedScene PointScene { get; set; }

	public override void _Ready()
	{
		base._Ready();
		if (Health is not null)
			Health.Damaged += SpawnPoints;
	}

	private void SpawnPoints(Entity entity, HitBox source, float damage)
	{
		if (Global.Player.Dead) return;
		var ratio = damage / Health.MaxHealth;
		var count = 1;
		var ratioPer = ratio / count;

		var th = (float)GD.RandRange(-Mathf.Pi / 12, Mathf.Pi / 12);
		var dir = source.GlobalPosition.DirectionTo(entity.GlobalPosition) * (120f + GD.RandRange(-10, 10));
		dir = dir.Rotated(th);

		var points = ratioPer * PointCount;
		for (int i = 0; i < count; i++)
		{

			var inst = PointScene.Instantiate<Point>();
			inst.Velocity = dir;
			inst.Position = entity.GlobalPosition;
			inst.Points = points;

			Global.Single.Spawn(inst);
		}
	}
	public void SpawnPointsRandom(Node2D source)
	{
		if (Global.Player.Dead) return;
		var ratio = 1f;
		var count = 20 + GD.Randi() % 10;
		var ratioPer = ratio / count;


		var points = ratioPer * PointCount;
		for (int i = 0; i < count; i++)
		{

			var th = (float)GD.RandRange(0, Mathf.Tau);
			var dir = Vector2.Right * (120f + GD.RandRange(-10, 10));
			dir = dir.Rotated(th);
			var inst = PointScene.Instantiate<Point>();
			inst.Velocity = dir;
			inst.Position = source.GlobalPosition;
			inst.Points = points;

			Global.Single.Spawn(inst);
		}
	}
}
