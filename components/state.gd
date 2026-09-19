@abstract
class_name State extends Node
var entity: Entity
func _ready() -> void:
	set_process(false);
	set_physics_process(false);
	entity = owner as Entity;
@warning_ignore_start("unused_signal")

signal transitioned(to: String);

@abstract func on_enter();
@abstract func on_exit();

@abstract func tick(delta: float);
@abstract func physics_tick(delta: float);
