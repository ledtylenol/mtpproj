using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Sounds : Node
{
	[Export]
	private Health Health { get; set; }

	[Export]
	private BulletShooter BulletShooter { get; set; }

	[Export]
	private Array<AudioStreamPlayer> SoundsOnHit { get; set; }

	[Export]
	private Array<AudioStreamPlayer> SoundsOnDeath { get; set; }

	[Export]
	private Array<AudioStreamPlayer> SoundsOnShoot { get; set; }
	public override void _Ready()
	{
		Health.Damaged += (h, s, d) => PlaySounds(SoundsOnHit);
		Health.Died += (e) => PlaySounds(SoundsOnDeath);
		if (BulletShooter is not null)
			BulletShooter.Shot += () => PlaySounds(SoundsOnShoot);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		foreach (var sound in SoundsOnHit)
		{
			sound.PitchScale = 0.1f + (float)Engine.TimeScale * 0.9f;
		}

		foreach (var sound in SoundsOnDeath)
		{
			sound.PitchScale = 0.1f + (float)Engine.TimeScale * 0.9f;
		}
	}
	private void PlaySounds(Array<AudioStreamPlayer> sounds)
	{
		foreach (var sound in sounds)
		{
			var dup = (AudioStreamPlayer)sound.Duplicate();
			Global.Single.Spawn(dup);
			dup.Play();
		}
	}
}
