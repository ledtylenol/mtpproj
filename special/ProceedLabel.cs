using Godot;
using System;

[GlobalClass]
public partial class ProceedLabel : RichTextLabel
{

	public override void _Ready()
	{
		base._Ready();
		LevelHandler.Single.LevelStarted += Hide;
		LevelHandler.Single.LevelFinished += ShowFinished;
		Global.Player.Died += ShowDead;
	}

	private void ShowFinished()
	{
		Show();
		Text = "[font_size=40][center][TAB]\n[shake]PROCEED";
	}

	private void ShowDead()
	{
		Show();
		Text = "[font_size=40][center]YOU DIED\n[TAB]\n[shake]PROCEED";
	}
}
