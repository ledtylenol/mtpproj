@tool
class_name GameWorld extends Node2D

@export
var size: Vector2:
	set(v):
		size = v
		update_play_area()
		queue_redraw()

var play_area: Rect2

@export
var color: Color:
	set(v):
		color = v
		queue_redraw()
		redraw.emit()
@export
var label: Label 

@export var player_health: Health 

@export var spawn_slot: Node2D 

@export var bg_handler: BgHandler
signal player_died(player: Player);


signal play_area_updated(newArea: Rect2);

@warning_ignore_start("unused_signal")
signal redraw();
var tween: Tween
func _enter_tree() -> void:
	if (Engine.is_editor_hint()): return;
	Global.world = self
func _ready() -> void:
	if (Engine.is_editor_hint()): return;

	update_play_area()
	if player_health:
		player_health.died.connect(player_died.emit) 
	Global.spawn_2d.connect(spawn_entity);
	Global.score_changed.connect(on_score_changed)
	on_score_changed(int(Global.run.score))
func update_play_area() -> void:
	play_area = Rect2(-size / 2, size);
	play_area_updated.emit(play_area);
func _physics_process(_delta: float) -> void:
	if (Engine.is_editor_hint()): return;
func on_score_changed(score: int) -> void:
	label.text = "%010d" % score;
func randomize_label() -> void:
	label.text = "%010d" % randi() 
func spawn_entity(e: Node) -> void:
	spawn_slot.add_child(e);
	e.owner = self
func reset() -> void:
	var player := Global.player;
	player_died.emit(Global.player);
	player.health.current_health = player.health.max_health;
	player.un_die();
	LevelHandler.current.reset();
	label.text = "%010d" % 0
	spawn_slot.queue_free();
	spawn_slot = Node2D.new()
	spawn_slot.y_sort_enabled = true
	add_child(spawn_slot);
	spawn_slot.owner = self
func tween_color(new_color: Color, duration: float) -> void:
	if tween: tween.kill()
	tween = TweenUtils.default_tween(self)
	tween.tween_property(self, "color", new_color, duration)
	print(new_color)
