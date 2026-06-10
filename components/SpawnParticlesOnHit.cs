using Godot;
using System;

[GlobalClass]
public partial class SpawnParticlesOnHit : Node
{
	[Export]
	public PackedScene Particles { get; set; }

	[Export]
	public Health Health { get; set; }

	[Export]
	public float Delay { get; set; } = 0f;

	public override void _Ready()
	{
		base._Ready();
		Health.Damaged += SpawnParticles;
	}

	private void SpawnParticles(Entity entity, HitBox source, float damage)
	{
		var dir = source.GlobalPosition.DirectionTo(entity.GlobalPosition);
		var entityTransform = entity.GlobalTransform;
		void func()
		{

			var inst = Particles.Instantiate<GpuParticles2D>();
			inst.Emitting = true;
			inst.OneShot = true;
			if (inst.ProcessMaterial is ParticleProcessMaterial pmm)
			{
				pmm.Direction = new Vector3(dir.X, dir.Y, 0f).Normalized();
			}
			inst.Transform = entityTransform;
			Global.Single.SpawnOther(inst);
		}

		if (Health.CurrentHealth > 0) return;
		if (Delay > 0f) GetTree().CreateTimer(Delay, false).Timeout += func;
		else func();

	}
}
