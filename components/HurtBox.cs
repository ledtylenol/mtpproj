using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class HurtBox : Area2D
{
	[Export]
	public Health Health { get; set; }

	private HashSet<HitBox> ExcludeList { get; set; } = [];

	public bool Hit(HitBox source)
	{
		if (!source.Pierce && ExcludeList.Contains(source)) return false;
		Health.Hit(this, source);

		return true;
	}
}
