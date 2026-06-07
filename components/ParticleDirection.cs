using Godot;
using System;

[GlobalClass]
public partial class ParticleDirection : Node
{
	[Export]
	public GpuParticles2D Particles { get; set; }
	[Export]
	public Entity Entity { get; set; }

	private ParticleProcessMaterial Material { get; set; }
	public override void _Ready()
	{
		base._Ready();
		Particles.ProcessMaterial = Particles.ProcessMaterial.Duplicate() as Material;
		Particles.PhysicsInterpolationMode = PhysicsInterpolationModeEnum.Off;
		Particles.TopLevel = true;
		Particles.Position = Entity.GlobalPosition.Snapped(new Vector2(1f, 1f));
		Material = Particles.ProcessMaterial as ParticleProcessMaterial;

		Particles.GetParent().CallDeferred("remove_child", Particles);
		Global.Single.CallDeferred("SpawnOther", Particles);
		UpdateDirection();
		Particles.Emitting = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		UpdateDirection();
	}
	private void UpdateDirection()
	{

		if (Material is ParticleProcessMaterial PMM)
		{
			var norm = -Entity.Velocity.Normalized();
			PMM.Direction = new Vector3(norm.X, norm.Y, 0f).Normalized();
		}
		Particles.Position = Entity.GlobalPosition.Snapped(new Vector2(1f, 1f));
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		Particles.QueueFree();
	}
}
