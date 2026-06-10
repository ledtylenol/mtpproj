using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class LeaderboardEntry : MarginContainer
{
	[Export]
	public Dictionary<string, Array<NodePath>> ColorRects { get; set; }

	[Export]
	public Label TimeLabel { get; set; }

	[Export]
	public Label ScoreLabel { get; set; }
	public override void _Ready()
	{
		base._Ready();

		foreach (var arr in ColorRects.Values)
			foreach (var path in arr)
			{
				var rect = GetNode<ColorRect>(path);
				rect.Hide();
			}
	}
	public void Initialize(Run run)
	{
		foreach (var key in run.Powerups.Keys)
		{
			var n = run.Powerups[key];
			var currentRects = ColorRects[key];
			for (int i = 0; i < n; i++)
			{
				var rect = GetNode<ColorRect>(currentRects[i]);
				rect.Show();
			}
		}
		TimeLabel.Text = $"{run.Time:N0}s";
		ScoreLabel.Text = $"{run.Score:N0}p";
	}
}
