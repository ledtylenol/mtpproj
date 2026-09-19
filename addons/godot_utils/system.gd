class_name System

enum Period {
	MORNING, NOON, AFTERNOON, EVENING, NIGHT, WITCHING_HOUR
}

static func check_app(app_name: String) -> bool:
	var output: Array = []
	var exitCode: int = 0

	match OS.get_name():
		"Windows":
			# The command lists tasks with a filter for our app name (plus .exe)
			exitCode = OS.execute("tasklist", ["/FI", "IMAGENAME eq " + app_name + ".exe", "/NH"], output)

		"macOS":
			exitCode = OS.execute("pgrep", ["-x", app_name], output)

		"Linux":
			exitCode = OS.execute("pgrep", ["-x", app_name], output)

		_:
			print("Unsupported OS for checking running applications")
			return false

	# If the command failed (exit code not equal to 0), return false.
	if exitCode != 0:
		return false

	# Process the output to check if the app is running.
	var results: String = output[0]
	print(results)
	if OS.get_name() == "Windows":
		return app_name.to_lower() in results.to_lower()
	else:
		return results.strip_edges() != ""

static func get_time() -> Period:
	var t := Time.get_datetime_dict_from_system()
	match t["hour"]:
		var x when x == 3: return Period.WITCHING_HOUR
		var x when x >= 21 or x <= 6: return Period.NIGHT
		var x when x <= 11: return Period.MORNING
		var x when x <= 13: return Period.NOON
		var x when x <= 16: return Period.AFTERNOON
		_: return Period.EVENING
