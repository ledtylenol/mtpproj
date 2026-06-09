using Godot;
using System;

[GlobalClass]
public partial class ParticleDirection : Node
{
	[Export]
	public GpuParticles2D Particles { get; set; }
	[Export]
	public Entity Entity { get; set; }

	[Export]
	public bool MoveToDrawWorld { get; set; }

	[Export]
	public bool UpdateDir { get; set; }

	private ParticleProcessMaterial Material { get; set; }
	public override void _Ready()
	{
		base._Ready();
		Particles.ProcessMaterial = Particles.ProcessMaterial.Duplicate() as Material;
		Particles.PhysicsInterpolationMode = PhysicsInterpolationModeEnum.Off;
		Particles.TopLevel = true;
		Particles.Position = Entity.GlobalPosition.Snapped(new Vector2(1f, 1f));
		Material = Particles.ProcessMaterial as ParticleProcessMaterial;

		if (UpdateDir)
			UpdateDirection();
		Particles.Emitting = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (!UpdateDir) return;
		if (Particles is null || !IsInstanceValid(Particles)) { QueueFree(); return; }
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
		if (!MoveToDrawWorld) return;
		base._ExitTree();
		Particles?.QueueFree();
	}
}
