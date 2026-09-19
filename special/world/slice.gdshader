shader_type canvas_item;

uniform sampler2D draw_mask: repeat_disable;
instance uniform vec2 dir1 = vec2(-1, 0);
instance uniform vec2 dir2 = vec2(1, 0);
instance uniform float displace_power = 1.0;

uniform sampler2D screen: hint_screen_texture, filter_nearest;
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	float l = texture(draw_mask, UV).r;
	vec2 displace = mix(normalize(dir1), normalize(dir2), l);
	vec3 col = texture(screen, SCREEN_UV + displace * displace_power).rgb;
	COLOR.rgb = col;
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
