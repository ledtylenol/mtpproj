using Godot;
using System;

[GlobalClass]
public partial class SpawnParticlesOnHit : Node
{
	[Export]
	public PackedScene Particles { get; set; }

	[Export]
	public Health Health { get; set; }

	public override void _Ready()
	{
		base._Ready();
		Health.Damaged += SpawnParticles;
	}

	private void SpawnParticles(Entity entity, HitBox source, float damage)
	{
		if (Health.CurrentHealth > 0) return;
		var inst = Particles.Instantiate<GpuParticles2D>();
		inst.Emitting = true;
		inst.OneShot = true;
		var dir = source.GlobalPosition.DirectionTo(entity.GlobalPosition);
		if (inst.ProcessMaterial is ParticleProcessMaterial pmm)
		{
			pmm.Direction = new Vector3(dir.X, dir.Y, 0f).Normalized();
		}
		inst.Transform = entity.GlobalTransform.Translated(-dir * 10f);
		Global.Single.SpawnOther(inst);
	}
}
