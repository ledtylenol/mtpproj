using Godot;
using System;

[GlobalClass]
public partial class EraseOffScreen : Node
{
	[Export]
	protected Node Target { get; set; }

	[Export]
	protected VisibleOnScreenNotifier2D Notifier { get; set; }

	public override void _Ready()
	{
		Notifier.ScreenExited += Erase;
	}

	public virtual void Erase()
	{
		Target.QueueFree();
	}
}
