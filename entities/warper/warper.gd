extends Entity
class_name Warper

@export var blink_sound: AudioStreamPlayer
@export var warp: Warp
@export var hit_scene: PackedScene
@export var music: AudioStreamPlayer
@export var expl_stats: ExplosionStats
var world_color: Color
var latest_color := Color.WHITE
func _ready() -> void:
	warp.teleported_to_dir.connect(on_teleport)
	warp.teleported_to_dir.connect(update_velocity)
	health.died.connect(
		func(_x):
			Global.world.tween_color(world_color, 10.0)
			Global.world.label.text = "DEATH"
			)
	world_color = Global.world.color
func _process(delta: float) -> void:
	super(delta)
	if music.get_playback_position() >= 115:
		music.play(10.12)
func on_teleport(v: Vector2) -> void:
	var parts = hit_scene.instantiate() as GPUParticles2D
	parts.emitting = true
	parts.transform = global_transform
	parts.rotation = v.angle()
	parts.modulate = latest_color
	Global.spawn_other(parts)

func spawn_cosmetic_explosion(color := Color.RED) -> void:
	var expl := Explosion.new(expl_stats, 0, 0, false)
	expl.position = position
	expl.visibility_layer = 1
	expl.top_level = true
	expl.color = color
	latest_color = color
	Global.spawn(expl);
	MusicHandler.play_boss_jingle()
func update_velocity(v: Vector2) -> void:
	velocity = v * 300.0
func move(_delta: float) -> void:
	var iterations = 5;
	var subdelt = _delta / iterations;

	for i in iterations:
		var res = move_and_collide(velocity * subdelt);

		if res:
			velocity = velocity.bounce(res.get_normal());
func change_color(col: Color) -> void:
	warp.color = col
	Global.world.tween_color(col, 1.0)
