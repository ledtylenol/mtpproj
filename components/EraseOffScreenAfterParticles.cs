using Godot;
using System;

[GlobalClass]
public partial class EraseOffScreenAfterParticles : EraseOffScreen
{
	[Export]
	public GpuParticles2D Particles { get; set; }
	public override void Erase()
	{
		Particles.Finished += Target.QueueFree;
		Particles.Emitting = false;
	}
}
