using Godot;
using System;

[GlobalClass]
public abstract partial class PowerupStat : Resource
{
	public abstract void Apply(Player player);
	public abstract string GetMessage();
	public abstract bool CanApply(Player player);
}
