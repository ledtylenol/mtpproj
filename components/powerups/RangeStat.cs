using Godot;
using System;

[GlobalClass]
public partial class RangeStat : PowerupStat
{
	public override void Apply(Player player)
	{
		player.Shooter.RangeItems += 1;
	}

	public override bool CanApply(Player player)
	{
		return player.Shooter.RangeItems < 4;
	}

	public override string GetMessage()
	{
		return $"Range Up";
	}
}
