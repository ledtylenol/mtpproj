using Godot;
using System;
using Godot.Collections;
using System.Linq;

[GlobalClass]
public partial class LevelPool : Resource
{
	[Export]
	public Array<Enemy> PossibleEnemies { get; set; } = [];

	[Export]
	public float MinimumCost { get; set; }

	[Export]
	public float MaximumCost { get; set; }

	[Export]
	public bool RandomizePosition { get; set; }

	public Enemy GetEnemy(float maxPoints)
	{
		Enemy enemy;
		var possibleEnemies = PossibleEnemies.Where((enemy) => enemy.Cost < maxPoints).ToList();
		var size = possibleEnemies.Count;
		if (size == 0) return null;
		else if (size == 1) enemy = possibleEnemies.First();
		else enemy = possibleEnemies[(int)(GD.Randi() % size)];
		return enemy;
	}
}
