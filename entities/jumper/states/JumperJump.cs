using Godot;
using System;

[GlobalClass]
public partial class JumperJump : JumperState
{
	[Export]
	public Node2D Shaker { get; set; }

	[Export]
	public AudioStreamPlayer Jump { get; set; }
	[Export]
	public AudioStreamPlayer Land { get; set; }
	[Export]
	public Timer JumpCooldown { get; set; }

	[Export]
	public Timer JumpTimer { get; set; }
	[Export]
	public Ghosts Ghosts { get; set; }

	[Export]
	public float MaxRandLength { get; set; }
	public override void OnEnter()
	{
		var mod = GD.Randi() % 2 == 0 ? 1f : 0f;
		var dir = Jumper.GlobalPosition.DirectionTo(Global.Player.GlobalPosition);
		var dist = Jumper.GlobalPosition.DistanceTo(Global.Player.GlobalPosition);
		var th = GD.RandRange(0, Mathf.Tau);
		var r = (float)GD.RandRange(0, MaxRandLength);
		var (sin, cos) = (Math.Sin(th), Math.Cos(th));

		var randVec = new Vector2((float)cos, (float)sin) * r;

		Jumper.Jump(GetDir() + randVec);

		Jump.Play();
		Jumper.Tween.Finished += () => EmitSignalTransitioned("idle");

		JumpCooldown.Start(Jumper.JumpTime - 0.1f);

		Ghosts.Active = true;
	}

	public override void OnExit()
	{
		if ((bool)Shaker.Get("is_playing")) Shaker.Call("force_stop_shake");
		Shaker.Call("play_shake");
		Jumper.HitBox.Active = false;
		Land.Play();

		Ghosts.Active = false;
		if (Jumper.Cooldown > 0f)
			JumpTimer.Start(Jumper.Cooldown * GD.RandRange(0.9, 1.1));
	}

	public override void PhysicsTick(double delta)
	{
		Jumper.MoveBounce(delta);
		Jumper.HitBox.Active = JumpCooldown.TimeLeft <= 0f;
		Jumper.HurtBox.Active = JumpCooldown.TimeLeft <= 0f;
	}

	public override void Tick(double delta)
	{

	}

	private Vector2 GetDir()
	{
		var playerPos = Global.Player.GlobalPosition;
		return (playerPos + Global.Player.Velocity * Jumper.JumpTime - Jumper.GlobalPosition) / Jumper.JumpTime;
	}

}
