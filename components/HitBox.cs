using Godot;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class HitBox : Area2D
{
	[Export]
	public Entity Entity { get; set; }

	[Export]
	public int HitLimit { get; set; } = 0;

	[Export]
	public bool Pierce { get; set; } = false;

	private int HitsLeft { get; set; }

	[Signal]
	public delegate void HitSucceededEventHandler(HitBox self, HurtBox hurtBox);

	[Signal]
	public delegate void HitFailedEventHandler(HitBox self, HurtBox hurtBox);

	public override void _Ready()
	{
		base._Ready();
		Entity ??= Owner as Entity;
		HitsLeft = HitLimit;
	}
	public IEnumerable<HurtBox> Collide()
	{
		var areas = GetOverlappingAreas().Where(area => area is HurtBox).Select(area => area as HurtBox);

		return areas;
	}
	public void Hit()
	{
		var collisions = new Queue<HurtBox>(Collide());
		if (collisions.Count == 0) return;
		if (HitLimit > 0)
		{
			while (HitsLeft > 0 && collisions.Count > 0)
			{
				var col = collisions.Dequeue();
				if (col.Hit(this))
					EmitSignalHitSucceeded(this, col);
				else
					EmitSignalHitFailed(this, col);
				HitsLeft--;
			}
		}
		else
		{
			while (collisions.Count > 0)
			{
				var col = collisions.Dequeue();
				if (col.Hit(this))
					EmitSignalHitSucceeded(this, col);
				else
					EmitSignalHitFailed(this, col);
			}
		}
	}
}
