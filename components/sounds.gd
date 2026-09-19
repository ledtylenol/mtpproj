class_name Sounds extends Node
@export var health: Health

@export var bullet_shooter: BulletShooter

@export var sounds_on_hit: Array[AudioStreamPlayer]

@export var sounds_on_death: Array[AudioStreamPlayer]

@export var sounds_on_shoot: Array[AudioStreamPlayer]
func _ready() -> void:
	health.damaged.connect(func(_e, _f, _d): play_sounds(sounds_on_hit))
	health.died.connect(func(_e): play_sounds(sounds_on_death))
	if bullet_shooter:
		bullet_shooter.shot.connect(func():play_sounds(sounds_on_shoot)) 

func _physics_process(_delta: float) -> void:
	for sound in sounds_on_hit:
		sound.pitch_scale = 0.1 + Engine.time_scale * 0.9;

	for sound in sounds_on_death:
		sound.pitch_scale = 0.1 + Engine.time_scale * 0.9;
func play_sounds(sounds: Array[AudioStreamPlayer]) -> void:
	for sound in sounds:

		var dup = sound.duplicate();
		Global.spawn(dup);
		dup.play();
