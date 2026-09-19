class_name SpawnWrapper extends Node2D
var node_to_spawn: Node2D 


func init(_node_to_spawn: Node2D):
	node_to_spawn = _node_to_spawn;
func _ready() -> void:
	push_error("Not implemented yet");
	spawn_the_node();

func spawn_the_node() -> void:
	Global.spawn(node_to_spawn);
	queue_free();
