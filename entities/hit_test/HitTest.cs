using Godot;
using System;

public partial class HitTest : Entity
{
	[Export]
	public ExplosionStats ExplosionStats { get; set; }
	public override void _Ready()
	{
		base._Ready();
		Health.Died += Die;
	}

	public void Die(Entity entity)
	{
		if (!Global.Player.Dead && LevelHandler.Single.Active)
		{
			var expl = new Explosion(ExplosionStats, ExplosionStats.MaskAll, ExplosionStats.LayerAll, true);
			var playerPos = Global.Player.GlobalPosition;
			expl.Position = playerPos;

			Global.Single.Spawn(expl);
		}
		QueueFree();
	}
}
