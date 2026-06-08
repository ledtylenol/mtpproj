shader_type canvas_item;

uniform sampler2D noise1: repeat_enable;
uniform sampler2D noise2: repeat_enable;
instance uniform float time;
uniform float step_start = 0.8;
uniform float step_offset = 0.2;
uniform float warp_speed;
instance uniform float uv_scale;
instance uniform vec2 flow_dir = vec2(1.0, 0.0);

uniform float warp_closeness : hint_range(0.01, 10.0) = 2.0;
uniform float warp_intensity : hint_range(0.0, 1.5)  = 0.12;
instance uniform vec3 color: source_color;
instance uniform float distance_threshold = 0.0;
void vertex() {
	
}
void fragment() {
	float t  = time * warp_speed;

	vec2 uv = UV * uv_scale;

	uv += flow_dir;
	float nx = uv.x / warp_closeness;
	float ny = uv.y / warp_closeness;
	vec2 dir = vec2(sin(t + ny * 2.0), sin(t + nx * 2.0));

	float warped_x = nx + warp_intensity * dir.x;
	float warped_y = ny + warp_intensity * dir.y;
	vec2 finalUV = vec2(warped_x, warped_y) * warp_closeness;
	vec3 col = texture(noise1, finalUV).rgb;
	float dist = mix(UV.y, 1.0, distance_threshold);
	float x = smoothstep(step_start , step_start + step_offset, col.r * dist);
	COLOR.a = step(0.5, x * dist);
	if (COLOR.a < 0.5) discard;
	COLOR.rgb = color;
}
