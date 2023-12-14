using Godot;

namespace Learnin;

public partial class HideMenu : Button
{
	private bool _left;
	
	public override void _Ready()
	{
	}
	
	public override void _Process(double delta)
	{
	}
	
	private void _on_button_down()
	{
		if (!_left)
		{
			this.RotationDegrees += 180;
			this.Position += new Vector2(48, 48);
			_left = true;
			GetNode<Polygon2D>("/root/Main/Menu").Position += new Vector2(420, 0);
		}
		else
		{
			this.RotationDegrees -= 180;
			this.Position -= new Vector2(48, 48);
			_left = false;
			GetNode<Polygon2D>("/root/Main/Menu").Position -= new Vector2(420, 0);
		}
	}
}
