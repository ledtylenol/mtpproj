using Godot;
using System;

[GlobalClass]
public partial class MoveToOtherworld : Node
{

	[Export]
	public GpuParticles2D Particles { get; set; }
	[Export]
	public bool UpdatePosition { get; set; }
	[Export]
	public Node2D Target { get; set; }
	public override void _Ready()
	{

		Particles.PhysicsInterpolationMode = PhysicsInterpolationModeEnum.Off;
		Particles.TopLevel = true;
		Particles.GetParent().CallDeferred("remove_child", Particles);
		Particles.VisibilityLayer = 2;
		Global.Single.CallDeferred("SpawnOther", Particles);
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (!UpdatePosition) return;

		Particles.GlobalPosition = Target.GlobalPosition.Snapped(Vector2.One);
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		Particles?.QueueFree();
	}
}
