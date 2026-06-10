using Godot;
using System;


public partial class MusicHandler : Node
{
	[Export]
	private AudioStreamPlayer FightPlayer { get; set; }

	[Export]
	private AudioStreamPlayer IdlePlayer { get; set; }

	[Export]
	private AudioStreamPlayer PausePlayer { get; set; }

	[Export]
	private AudioStreamPlayer FightJingle { get; set; }

	[Export]
	private AudioStreamPlayer IdleJingle { get; set; }

	[Export]
	private AudioStreamPlayer BossJingle { get; set; }

	[Export]
	private Timer FightTimer { get; set; }

	[Export]
	private Timer IdleTimer { get; set; }

	private Tween IdleTween { get; set; }

	public static MusicHandler Single { get; set; }

	private float LinearIdleVolume;

	public override void _Ready()
	{
		base._Ready();
		Single = this;
		LevelHandler.Single.LevelFinished += StartIdle;
		LevelHandler.Single.LevelStarted += StartFight;
		LevelHandler.Single.BossNextTurn += StartBoss;
		FightTimer.Timeout += () => FightPlayer.Play();
		IdleTimer.Timeout += () => IdlePlayer.Play();
		IdleTimer.Timeout += TweenIdleVolume;
		StartIdle();
		LinearIdleVolume = IdlePlayer.VolumeLinear;
		PauseManager.Single.Paused += () => PausePlayer.Play();
		PauseManager.Single.UnPaused += PausePlayer.Stop;
	}

	private void StartIdle()
	{
		FightPlayer.Stop();
		FightTimer.Stop();
		IdleJingle.Play();
		IdleTimer.Start(GD.RandRange(2.2, 4.4));
	}

	private void StartFight()
	{
		IdlePlayer.Stop();
		IdleTimer.Stop();
		FightJingle.Play(0.36f);
		FightTimer.Start(0.001);
	}
	private void TweenIdleVolume()
	{
		IdleTween?.Kill();
		IdleTween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);

		IdleTween.TweenProperty(IdlePlayer, "volume_linear", LinearIdleVolume, 1f).From(0f);
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		FightPlayer.PitchScale = 0.1f + (float)Engine.TimeScale * 0.9f;
	}
	public void StartBoss()
	{
		FightTimer.Stop();
		FightPlayer.Stop();
		IdleTimer.Stop();
		IdlePlayer.Stop();
		BossJingle.Play();
	}
}
