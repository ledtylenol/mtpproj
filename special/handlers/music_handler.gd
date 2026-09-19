extends Node
@export var fight_player: AudioStreamPlayer  

@export var idle_player: AudioStreamPlayer  

@export var pause_player: AudioStreamPlayer  

@export var fight_jingle: AudioStreamPlayer  

@export var idle_jingle: AudioStreamPlayer  

@export var boss_jingle: AudioStreamPlayer  

@export var fight_timer: Timer  

@export var idle_timer: Timer  

var idle_tween: Tween 

var linear_idle_volume: float;

func _ready() -> void:
	await World.single.scene_load_ended
	LevelHandler.current.level_finished.connect(start_idle)
	LevelHandler.current.level_started.connect(start_fight)
	LevelHandler.current.boss_next_turn.connect(start_boss)
	fight_timer.timeout.connect(fight_player.play)
	idle_timer.timeout.connect(idle_player.play)
	idle_timer.timeout.connect(tween_idle_volume)
	start_idle();
	linear_idle_volume = idle_player.volume_linear;
	PauseManager.paused.connect(pause_player.play)
	PauseManager.un_paused.connect(pause_player.stop)
	World.single.scene_load_ended.connect(start_idle)
func start_idle() -> void:
	fight_player.stop();
	fight_timer.stop();
	idle_jingle.play();
	idle_timer.start(randf_range(2.2, 4.4));

func start_fight() -> void:
	idle_player.stop();
	idle_timer.stop();
	fight_jingle.play(0.36);
	if not LevelHandler.current.is_boss():
		fight_timer.start(0.001);
func tween_idle_volume() -> void:
	if idle_tween: idle_tween.kill();
	idle_tween = TweenUtils.default_tween(self)

	idle_tween.tween_property(idle_player, "volume_linear", linear_idle_volume, 1).from(0);
func _physics_process(_delta: float) -> void:
	fight_player.pitch_scale = 0.1 + Engine.time_scale * 0.9;
func start_boss() -> void:
	fight_timer.stop();
	fight_player.stop();
	idle_timer.stop();
	idle_player.stop();
	boss_jingle.play();

func play_boss_jingle() -> void:
	boss_jingle.play()
