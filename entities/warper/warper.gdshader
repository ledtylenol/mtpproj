shader_type canvas_item;

uniform sampler2D noise: repeat_enable;
uniform float quant = 10;
uniform float displace_power = 0.1;
uniform sampler2D tex;
instance uniform float time = 0;
uniform sampler2D screen: hint_screen_texture;
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec2 uv = round(UV * quant) / quant;
	float n = texture(noise, uv + time).r * 2. - 1.;
	vec2 pushed = UV + n * normalize(UV - vec2(0.5)  * displace_power);
	vec2 sc_pushed = SCREEN_UV + n * normalize(UV - vec2(0.5)  * displace_power);
	vec4 s_col = texture(screen, sc_pushed);
	s_col.rgb = 1.0 - s_col.rgb;
	vec4 col = texture(TEXTURE, pushed);
	COLOR = mix(col, s_col, 1.0 - col.a);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
