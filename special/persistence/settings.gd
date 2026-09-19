class_name Settings extends Resource
@export_range(0.0, 1.0, 0.1)
var sfx_volume := 0.5

@export_range(0.0, 1.0, 0.1)
var music_volume := 0.5
@warning_ignore_start("unused_signal")

signal sfx_changed(new_sfx: float)
signal music_changed(new_music: float)

func save() -> void:
	ResourceSaver.save(self, "user://settingsgd.tres");
static func load() -> Settings:
	if (!ResourceLoader.exists("user://settingsgd.tres")): return null;

	return ResourceLoader.load("user://settingsgd.tres");

func set_sfx(value: float) -> void:
	sfx_volume = value;
	var sfx_bus = AudioServer.get_bus_index("Sfx");
	AudioServer.set_bus_volume_linear(sfx_bus, value);
func set_music(value: float) -> void:
	music_volume = value;
	var  music_bus= AudioServer.get_bus_index("Music");
	AudioServer.set_bus_volume_linear(music_bus, value);
