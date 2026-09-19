extends Node
var world: GameWorld 
var player: Player 
var settings: Settings 
var run: Run
var runs: RunsHistory
signal spawn_2d(source: Node)
signal spawn_other_world(source: Node2D)
signal spawn_ui(source: Control)
@warning_ignore_start("unused_signal")

signal entity_died(e: Entity, culprit: Entity)
signal run_ended(run: Run)
signal boss_phase_changed
signal score_changed(new_score: int)
var counting := false

func _enter_tree() -> void:
	runs = RunsHistory.load();
	if (not runs): runs = RunsHistory.new();
	for runn in runs.runs:
		print("run with %s score lasted %ss" % [runn.score, runn.time]);

	settings = Settings.load();
	if (not settings): settings = Settings.new();
	run = Run.new();

func _ready() -> void:
	call_deferred("connect_handlers");

func connect_handlers() -> void:
	LevelHandler.current.level_finished.connect(stop_counting)
	LevelHandler.current.level_started.connect(start_counting)
	player.health.died.connect(submit_run.unbind(1))

func spawn(src: Node) -> void:
	if (src is Node2D): 
		spawn_2d.emit(src);
	elif (src is Control): spawn_ui.emit(src);
	else: add_child(src);

func force_spawn_2d(src: Node) -> void:
	spawn_2d.emit(src)

func spawn_other(src: Node2D) -> void:
	spawn_other_world.emit(src)


func _process(delta: float) -> void:
	if counting:
		run.time += delta;
func stop_counting() -> void:
	counting = false;

func start_counting() -> void:
	counting = true;

func submit_run() -> void:
	runs.add_run(run);
	runs.save();
	run_ended.emit(run);
	run = Run.new();
	score_changed.emit(0)
