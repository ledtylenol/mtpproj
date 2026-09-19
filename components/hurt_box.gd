class_name HurtBox extends Area2D
@export var health: Health

@export var active: bool = true;

var exclude_list: Dictionary[HitBox, float] = {}

func hit(source: HitBox ) -> bool:
	if (exclude_list.has(source)): return false;
	if (!active): return false;
	health.hit(self, source);
	if (source.pierce): exclude_list[source] = health.i_frames
	else: exclude_list[source] = maxf(source.i_frames, health.i_frames);
	return true;

func _physics_process(delta: float) -> void:
	var d_q = []
	d_q.resize(exclude_list.size())
	for key in exclude_list.keys():
		if (not key or exclude_list[key] < 0.0):
			d_q.push_back(key);
			continue
		exclude_list[key] -= delta;

	# buffer removes because removing while indexing is bad
	for hit_box in d_q: 
		if hit_box:
			exclude_list.erase(hit_box);
