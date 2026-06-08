using Godot;
using System;

[GlobalClass]
public partial class PointApplier : Node
{
	[Export]
	public Point Point { get; set; }
	public bool Active { get; set; } = true;

	public override void _Ready()
	{
		base._Ready();
	}


	public void ApplyPoints()
	{
		if (!IsInstanceValid(Owner)) return;
		Global.Single.Score += Point.Points;
		//TODO! Add sound here
		Owner.QueueFree();
		ShepardSound.Single.Hit();
	}
}
