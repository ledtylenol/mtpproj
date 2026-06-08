using Godot;
using System;

[GlobalClass]
public partial class ShepardSound : AudioStreamPlayer

{
	public static ShepardSound Single { get; set; }
	private float Heat { get; set; } = 0f;


	public override void _Ready()
	{
		base._Ready();
		Single = this;
	}
	public void Hit()
	{
		Heat += 0.1f;
		PitchScale = 1f + Mathf.Log(1f + Heat);
		Play();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		Heat = Mathf.MoveToward(Heat, 0f, (float)delta);
	}
}
