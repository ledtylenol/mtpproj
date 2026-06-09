using Godot;
using System;

[GlobalClass]
public partial class Powerup : Area2D
{
	[Export]
	public PackedScene Particles { get; set; }

	[Export]
	public PowerupStat Stat { get; set; }

	[Export]
	public bool Active { get; set; } = true;

	[Export]
	public Points Points { get; set; }
	[Signal]
	public delegate void PowerupAppliedEventHandler(string message);
	public override void _Ready()
	{
		base._Ready();
		Connect("body_entered", Callable.From<Node2D>(OnBodyEntered));
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (!Active) return;
		if (body is not Player player) return;

		var couldApply = Stat.CanApply(player);
		if (!couldApply)
		{
			Points.SpawnPointsRandom(this);
			Active = false;
			SpawnParticles();
			EmitSignalPowerupApplied("+500p");
			return;
		}

		Stat.Apply(player);
		EmitSignalPowerupApplied(Stat.GetMessage());
		Active = false;
	}
	public void ActivateAfter(float time)
	{
		GetTree().CreateTimer(time).Timeout += () => Active = true;
	}

	private void SpawnParticles()
	{
		var inst = Particles.Instantiate<GpuParticles2D>();
		inst.Emitting = true;
		inst.OneShot = true;
		inst.Transform = GlobalTransform;
		Global.Single.SpawnOther(inst);
	}
}
