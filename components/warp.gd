extends Node2D
class_name Warp

@export var entity: Node2D
@export var sprite_to_copy: Sprite2D
@export var ghost_interval := 20.0
@export var initial_lifetime := 0.5
@export var total_ghost_time := 0.2
@export var quant := 5
@export var start_width := 10.0
@export var color := Color.GHOST_WHITE
var time := randf_range(-10, 10)
var lines: Dictionary[Tween, PackedVector2Array]
var tweens: Dictionary[Tween, float]
signal teleported
signal teleported_to_dir(dir: Vector2)
func blink(to: Vector2) -> void:
	teleport(to)

func relative(offset: Vector2) -> void:
	teleport(entity.position + offset)

func teleport(pos: Vector2) -> void:
	var start := entity.position
	var end := pos
	
	var ghost_count := int(start.distance_to(end) / ghost_interval) + 1
	var tween: Tween = TweenUtils.default_tween(self, true)
	var delay := 0.0
	var delay_per: float = total_ghost_time / ghost_count
	for i in ghost_count:
		var subt := TweenUtils.default_tween(self)
		var rel: float = float(i) / ghost_count
		var ghost_p = start.lerp(end, rel)
		
		var sprite := Ghost.new(initial_lifetime, quant, delay);

		sprite.texture = sprite_to_copy.texture
		sprite.transform = sprite_to_copy.global_transform
		sprite.position = ghost_p
		sprite.flip_h = sprite_to_copy.flip_h
		subt.tween_callback(Global.spawn.bind(sprite))
		subt.parallel().tween_property(sprite, "scale:x", 0.0, initial_lifetime)
		subt.parallel().tween_property(sprite, "scale:y", 0.0, initial_lifetime * 4)
		tween.tween_subtween(subt)
		delay += delay_per
	lines[tween] = [start, end]
	tweens[tween] = start_width
	tween.tween_method(tween_width.bind(tween), start_width, 0.0, initial_lifetime)
	tween.chain().tween_callback(remove_line.bind(tween))
	entity.position = end
	entity.reset_physics_interpolation()
	teleported.emit()
	teleported_to_dir.emit(start.direction_to(end))
func tween_width(width: float, tween: Tween) -> void:
	tweens[tween] = width
	queue_redraw()
func remove_line(tween: Tween) -> void:
	lines.erase(tween)
	tweens.erase(tween)
	queue_redraw()

func _draw() -> void:
	for tween: Tween in lines.keys():
		var points: PackedVector2Array = lines[tween]
		var width := maxf(tweens[tween], 0)
		draw_line(points[0], points[1], color, width, false)
		draw_circle(points[1], width / 2, Color.WHITE_SMOKE, false, width)

func _process(delta: float) -> void:
	time += delta
