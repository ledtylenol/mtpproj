extends Area2D
class_name WorldWeapon

@export var scene: PackedScene
@export var texture: Texture
@export var sprite: Sprite2D
var start_vec := Vector2.ZERO
var start_rot_vel := randf_range(PI, PI * 5)
func _ready() -> void:
	start_rot_vel *= -1 if randi() % 2 == 0 else 1
	sprite.texture = texture
	var r := randf_range(70, 111)
	var th := randf_range(0, TAU)
	start_vec = Vector2(cos(th), sin(th)) * r
	area_entered.connect(on_area_entered)
func _physics_process(delta: float) -> void:
	position += start_vec * delta
	start_vec = start_vec.move_toward(Vector2.ZERO, delta * 111)
	sprite.rotate(start_rot_vel * delta)
	start_rot_vel = M.smooth_nudgef(start_rot_vel, 0.0, 3, delta)

func on_area_entered(area: Area2D) -> void:
	if area is WeaponAcceptor:
		var inst = scene.instantiate()
		area.weapon_manager.add_child(inst)
		queue_free()
