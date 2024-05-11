using Godot;

namespace Learnin;

public partial class SmolNode : Polygon2D
{
	private int _state;
	private int _id;
	private bool _inGame;
	
	public override void _Ready()
	{
		this._state = 0;
		this.Color = Colors.SkyBlue;
	}
	
	public override void _Process(double delta)
	{
	}
	
	private void _on_area_2d_input_event(Node viewport, InputEvent @event, long shape_idx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left)
		{
			if (_inGame)
			{
				if (this.Color == Colors.SkyBlue)
				{
					this.Color = Colors.PaleGreen;
				}
				else
				{
					this.Color = Colors.SkyBlue;
				}
			}
			else
			{
				if (this.Color == Colors.SkyBlue)
				{
					this.Color = Colors.PaleVioletRed;
				}
				else
				{
					this.Color = Colors.SkyBlue;
				}
			}
		}
	}
}
