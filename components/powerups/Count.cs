using Godot;
using System;

[GlobalClass]
public partial class Count : PowerupStat
{
	public override void Apply(Player player)
	{
		player.Shooter.CountItems += 1;
	}
	public override bool CanApply(Player player)
	{
		return player.Shooter.CountItems < 2;
	}

	public override string GetMessage()
	{
		return "Bullet count up";
	}
}
