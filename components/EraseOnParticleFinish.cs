using Godot;
using System;

[GlobalClass]
public partial class EraseOnParticleFinish : Node
{
	public override void _Ready()
	{
		base._Ready();
		var parts = GetParent<GpuParticles2D>();
		if (parts is null)
		{
			GD.PushError("Parent must be particles");
			QueueFree();
		}
		parts.Finished += parts.QueueFree;
	}
}
