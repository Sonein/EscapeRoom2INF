using Godot;
using System;

namespace Learnin;

public class Login : Polygon2D
{

	public override void _Ready()
	{
	}

	public override void _Process(float delta)
	{
	}

	private void OnButtonButtonDown()
	{
		foreach (var x in this.GetParent().GetChildren())
		{
			if (x is CanvasItem x1)
			{
				x1.Visible = true;
			}
		}
		this.Visible = false;
	}
}
