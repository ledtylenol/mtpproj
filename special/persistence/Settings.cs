using Godot;
using System;

[GlobalClass]
public partial class Settings : Resource
{
	[Export(PropertyHint.Range, "0, 1, 0.1")]
	public float SfxVolume { get; private set; } = 0.5f;

	[Export(PropertyHint.Range, "0, 1, 0.1")]
	public float MusicVolume { get; private set; } = 0.5f;

	[Signal]
	public delegate void SfxChangedEventHandler(float newSfx);

	[Signal]
	public delegate void MusicChangedEventHandler(float newSfx);

	public void Save()
	{
		ResourceSaver.Save(this, "user://settings.tres");
	}
	public static Settings Load()
	{
		if (!ResourceLoader.Exists("user://settings.tres")) return null;

		return ResourceLoader.Load<Settings>("user://settings.tres");
	}

	public void SetSfx(float value)
	{
		SfxVolume = value;
		var sfxBus = AudioServer.GetBusIndex("Sfx");
		AudioServer.SetBusVolumeLinear(sfxBus, value);
	}
	public void SetMusic(float value)
	{
		MusicVolume = value;
		var musicBus = AudioServer.GetBusIndex("Music");
		AudioServer.SetBusVolumeLinear(musicBus, value);
	}
}
