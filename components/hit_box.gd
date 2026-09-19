class_name HitBox extends Area2D
@export var entity: Entity

@export var hit_limit: int = 0;

@export var pierce: bool = false;

@export var damage: int = 1;

@export var active: bool = true;
@export var i_frames := 0.5
var hits_left: int 

signal hit_succeeded(hb: HitBox, hurt_box: HurtBox);

signal hit_failed(hb: HitBox, hurt_box: HurtBox);

signal out_of_hits(hb: HitBox);

func _ready() -> void:
	if (not entity): entity = owner as Entity
	hits_left = hit_limit;

func area_is_hitbox_and_not_owned_by_owner_of_this_hitbox(area: Area2D) -> bool:
	return area is HurtBox && area.owner != entity

func map_area_2d_to_hurtbox(area: Area2D) -> HurtBox:
	return area as HurtBox
func collide() -> Array:
	var areas = get_overlapping_areas().filter(area_is_hitbox_and_not_owned_by_owner_of_this_hitbox).map(map_area_2d_to_hurtbox);

	return areas;
func _physics_process(_delta: float) -> void:
	hit();
func hit() -> void:
	if (!active): return;
	var collisions = collide()
	if (collisions.is_empty()): return;
	if (hit_limit > 0):
		while (hits_left > 0 and not collisions.is_empty()):
			var col = collisions.pop_back();
			if (col.hit(self)):
				hit_succeeded.emit(self, col);
				hits_left -= 1;
			else:
				hit_failed.emit(self, col);
		if (hits_left == 0):
			out_of_hits.emit(self);
	else:
		while (not collisions.is_empty()):
			var col = collisions.pop_back();
			if (col.hit(self)):
				hit_succeeded.emit(self, col);
			else:
				hit_failed.emit(self, col);
