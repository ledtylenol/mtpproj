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

	public override void _Ready()
	{
		base._Ready();
		Health.Died += SpawnPoints;
	}

	private void SpawnPoints(Entity entity)
	{
		if (Global.Player.Dead) return;
		List<uint> digits = [];
		while (PointCount > 0)
		{
			digits.Add(PointCount % 10);
			PointCount /= 10;
		}

		digits.Reverse();
		var length = digits.Count;
		for (int i = 0; i < length; i++)
		{
			var value = Mathf.Pow(10, length - i - 1);
			GD.Print($"{digits[i] * value}, ");
		}
		GD.Print();
	}
}
