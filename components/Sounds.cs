using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Sounds : Node
{
	[Export]
	private Health Health { get; set; }

	[Export]
	private Array<AudioStreamPlayer> SoundsOnHit { get; set; }

	[Export]
	private Array<AudioStreamPlayer> SoundsOnDeath { get; set; }

	public override void _Ready()
	{
		Health.Damaged += (h, s, d) => PlaySounds(SoundsOnHit);
		Health.Died += (e) => PlaySounds(SoundsOnDeath);
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
