shader_type canvas_item;

instance uniform float alpha = 1.0;
uniform sampler2D noise: repeat_enable, filter_nearest;
uniform vec2 dir;
uniform vec2 scale;
instance uniform float time = 0.0;
instance uniform float vert_thres = 0.0;
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	float gradval = mix(UV.y, 1.0, vert_thres);
	vec2 uv = UV + dir * time;
	float val = texture(noise, uv * scale).r ;
	COLOR.a = step(val, gradval * alpha);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
