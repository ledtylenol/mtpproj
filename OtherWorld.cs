using Godot;
using System;

[GlobalClass]
public partial class OtherWorld : SubViewportContainer
{
	[Export]
	public Node2D Root { get; set; }

	[Export]
	public SubViewport Viewport { get; set; }
	public override void _Ready()
	{
		base._Ready();
		Global.Single.SpawnOtherWorld += SpawnNode;
		Global.Single.LevelChanged += ClearViewport;
		Global.Single.World.PlayAreaUpdated += UpdateSize;
		UpdateSize(Global.Single.World.PlayArea);
	}

	private void SpawnNode(Node2D node)
	{
		Root.AddChild(node);
	}
	private void ClearViewport()
	{
		foreach (var child in Root.GetChildren()) if (child is Node2D node) node.Hide();
		Viewport.RenderTargetClearMode = SubViewport.ClearMode.Once;

	}

	private void UpdateSize(Rect2 newSize)
	{
		Position = newSize.Position;
		Size = newSize.Size;
	}
}
