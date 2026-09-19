class_name PointIdle extends State
func _ready() -> void:
	super()
	LevelHandler.current.level_finished.connect(on_level_change, 4);

func physics_tick(delta: float) -> void:
	entity.velocity = entity.velocity.move_toward(Vector2.ZERO, entity.friction * delta);
	entity.move(delta);

func on_level_change() -> void:
	transitioned.emit("Trace");

func on_enter():
	pass

func on_exit():
	pass
func tick(_delta: float):
	pass
