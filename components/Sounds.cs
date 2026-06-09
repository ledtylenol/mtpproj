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
