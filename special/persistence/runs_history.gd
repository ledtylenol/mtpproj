class_name RunsHistory extends Resource
@export var runs: Array[Run] = []


func save() -> void:
	ResourceSaver.save(self, "user://runsgd.tres");
static func load() -> RunsHistory:
	if (!ResourceLoader.exists("user://runsgd.tres")): return null;
	return ResourceLoader.load("user://runsgd.tres");

func add_run(run: Run) -> void:
	runs.push_back(run);
