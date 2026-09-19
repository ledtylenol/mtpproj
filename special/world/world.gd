extends Node
class_name World

@export var current_world: GameWorld
@export var start_scene: PackedScene
var scene_queue: Array[PackedScene]
@warning_ignore_start("unused_signal")
signal scene_load_started
signal scene_load_ended

static var single: World
func _enter_tree() -> void:
	single = self

func _ready() -> void:
	push_world(start_scene)
func push_world(scene: PackedScene) -> void:
	scene_load_started.emit()
	var pack := PackedScene.new()
	pack.pack(current_world)
	scene_queue.push_back(pack)
	if current_world:
		current_world.queue_free()
	var inst := scene.instantiate()
	add_child(inst)
	current_world = inst
	scene_load_ended.emit()
	
func pop_world() -> void:
	if scene_queue.is_empty(): 
		push_warning("can't pop. queue empty")
		return
	scene_load_started.emit()
	var scene = scene_queue.pop_back()
	if current_world:
		current_world.queue_free()
	current_world.queue_free()
	var inst: GameWorld = scene.instantiate()
	add_child(inst)
	scene_load_ended.emit()
	current_world = inst

func _input(event: InputEvent) -> void:
	if  event.is_action("light") and event.is_pressed():
		push_world(start_scene)
	if event.is_action("heavy") and event.is_pressed():
		pop_world()
