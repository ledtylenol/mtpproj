using Godot;
using System;

public partial class WorldBorder : StaticBody2D
{
	[Export]
	private World2d World { get; set; }

	[Export]
	private SegmentShape2D Left { get; set; }
	[Export]
	private SegmentShape2D Right { get; set; }
	[Export]
	private SegmentShape2D Up { get; set; }
	[Export]
	private SegmentShape2D Down { get; set; }


	public override void _Ready()
	{
		base._Ready();
		World.PlayAreaUpdated += UpdateShapes;
		UpdateShapes(World.PlayArea);
	}


	private void UpdateShapes(Rect2 newArea)
	{
		var dX = -newArea.Size.X / 2f;
		var dY = -newArea.Size.Y / 2f;

		var vert = new Vector2(0f, dY);
		var hor = new Vector2(dX, 0f);

		Left.A = -hor - vert;
		Left.B = -hor + vert;

		Right.A = hor - vert;
		Right.B = hor + vert;

		Up.A = -hor - vert;
		Up.B = hor - vert;

		Down.A = -hor + vert;
		Down.B = hor + vert;
	}
}
