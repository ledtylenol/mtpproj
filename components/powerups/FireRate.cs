using Godot;
using System;

[GlobalClass]
public partial class FireRate : PowerupStat
{
	public override void Apply(Player player)
	{
		player.Shooter.CooldownItems += 1;
	}

	public override bool CanApply(Player player)
	{
		return player.Shooter.CooldownItems < 4;
	}

	public override string GetMessage()
	{
		return "Firerate up";
	}

}
