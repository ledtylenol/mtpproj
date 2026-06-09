using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class HealthDisplay : HBoxContainer
{
	[Export]
	private PackedScene HealthIconScene { get; set; }

	[Export]
	public float InitialDelay { get; set; }

	[Export]
	public float DelayPerIcon { get; set; }

	private List<HealthIcon> icons = [];

	private Tween Tween { get; set; }

	public override void _Ready()
	{
		base._Ready();
		var delay = InitialDelay;
		Global.Player.Health.Died += OnPlayerDeath;
		for (int i = 0; i < Global.Player.Health.MaxHealth - 1; i++)
		{
			var inst = HealthIconScene.Instantiate<HealthIcon>();
			AddChild(inst);
			delay += DelayPerIcon;
			icons.Add(inst);
		}
		UpdateIcons();

		Global.Player.Health.HealthChanged += OnHealthChanged;
	}

	private void OnHealthChanged(Entity entity, float oldHealth, float newHealth)
	{
		UpdateIcons();
	}

	private void UpdateIcons()
	{
		Tween?.CustomStep(9999f);
		Tween?.Kill();
		Tween = CreateTween().SetParallel();
		var delay = InitialDelay;
		var health = Global.Player.Health;
		for (int i = 0; i < Global.Player.Health.MaxHealth - 1; i++)
		{
			var icon = icons[i];
			if (i < health.CurrentHealth - 1 && !icon.Filled)
			{
				Tween.TweenSubtween(icon.ShowIcon()).SetDelay(delay);
				delay += DelayPerIcon;

			}
			if (i >= health.CurrentHealth - 1 && icon.Filled)
			{
				GD.Print("LOST HEALTH");
				Tween.TweenSubtween(icon.HideIcon()).SetDelay(delay);
				delay += DelayPerIcon;
			}
		}
		if (delay == InitialDelay) Tween.Kill();
	}
	private void OnPlayerDeath(Entity entity)
	{
		Tween?.Kill();
		Tween = CreateTween().SetParallel();
		var delay = 0.25f;

		var health = Global.Player.Health;
		for (int i = 0; i < health.MaxHealth - 1; i++)
		{
			var icon = icons[i];
			Tween.TweenSubtween(icon.MinimizeIcon()).SetDelay(delay);
			delay += 0.15f;
		}
	}
}
