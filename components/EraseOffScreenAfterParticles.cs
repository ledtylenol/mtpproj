using Godot;
using System;

[GlobalClass]
public partial class EraseOffScreenAfterParticles : EraseOffScreen
{
	[Export]
	public GpuParticles2D Particles { get; set; }
	public override void Erase()
	{
		GD.Print("ERASE");
		GetTree().CreateTimer(Particles.Lifetime).Timeout += Target.QueueFree;
		Particles.Emitting = false;
	}
}
