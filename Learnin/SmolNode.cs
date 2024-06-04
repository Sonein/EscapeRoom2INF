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
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.IsPressed())
		{
			if (_inGame)
			{
				if (this.Color == Colors.SkyBlue)
				{
					//TODO iba ak je v chaine
					this.Color = Colors.PaleGreen;
					this._state = 2;
				}
				else
				{
					this.Color = Colors.SkyBlue;
					this._state = 0;
				}
			}
			else
			{
				if (this.Color == Colors.SkyBlue)
				{
					this.Color = Colors.PaleVioletRed;
					this._state = 1;
				}
				else
				{
					this.Color = Colors.SkyBlue;
					this._state = 0;
				}
			}
		}
	}

	private void SetSpecial(string special)
	{
		this._id = int.Parse(special);
	}

	private int GetState()
	{
		return this._state;
	}
	
	private void SetState(int ina)
	{
		this._state = ina;
	}
}
