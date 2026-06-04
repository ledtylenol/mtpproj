using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class HurtBox : Area2D
{
	[Export]
	public Health Health { get; set; }

	private Dictionary<HitBox, double> ExcludeList { get; set; } = [];

	public bool Hit(HitBox source)
	{
		if (ExcludeList.ContainsKey(source)) return false;
		Health.Hit(this, source);
		if (source.Pierce) ExcludeList.Add(source, Health.IFrames);
		else ExcludeList.Add(source, 9999999f);
		return true;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		var dQ = new List<HitBox>(ExcludeList.Count);
		foreach (var key in ExcludeList.Keys)
		{
			ExcludeList[key] -= delta;
			if (key is null || ExcludeList[key] < 0.0) dQ.Add(key);
		}

		// buffer removes because removing while indexing is bad
		foreach (var hitBox in dQ) ExcludeList.Remove(hitBox);
	}
}
