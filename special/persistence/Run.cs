using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class Run : Resource
{
	[Export]
	public float Time { get; set; } = 0f;

	[Export]
	public float Score { get; set; } = 0f;

	[Export]
	public Dictionary<string, uint> Powerups { get; set; } = [];

	public void AddPowerup(string powerupName)
	{
		var count = 1u;
		if (Powerups.TryGetValue(powerupName, out uint value))
		{
			count += value;
		}
		Powerups[powerupName] = count;
	}
}
