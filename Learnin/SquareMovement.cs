using Godot;

namespace Learnin;

public partial class SquareMovement : Polygon2D
{
	private bool _isSelected;
	private bool _isDragging;
	private bool _inGame;
	private Vector2 _ironMouseOffset;
	private MovementManager _movementManager;
	
	public override void _Ready()
	{
		_movementManager = MovementManager.Instance;
		_movementManager.Add(this);
	}
	
	public override void _Process(double delta)
	{
		if (_isDragging)
		{
			_isDragging = _movementManager.CanMove(this);
			FollowIronMouse();
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey && eventKey.Pressed && _isSelected)
		{
			switch (eventKey.Keycode)
			{
				case Godot.Key.D:
					SigKill();
					break;
			}
		}
	}

	private void FollowIronMouse()
	{ 
		Position = GetGlobalMousePosition() + _ironMouseOffset;
	}


	private void SetInternals(string x)
	{
		switch (x)
		{
			case "select":
				_isSelected = !_isSelected;
				break;
			case "drag":
				_isDragging = !_isDragging;
				break;
			case "play":
				_inGame = !_inGame;
				break;
		}
	}
	
	private void _on_area_2d_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (!_inGame)
		{
			if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left)
			{
				_isDragging = _movementManager.StartMove(@event, GetGlobalMousePosition(), Position, this);
				if (_isDragging)
				{
					_ironMouseOffset = _movementManager.IronmouseOffset;
				}
			}
			else if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Right)
			{
				if (@event.IsPressed())
				{
					SetInternals("select");
				}
			}
			if (@event is InputEventMouseButton { DoubleClick: true } && !_inGame)
			{
				GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("ClearSelf");	
				GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ClearSelf");
			}
		}
	}

	private string GetShapeType()
	{
		return "none";
	}

	private void SigKill()
	{
		GetNode<MenuButton>("/root/Main/Menu/ItemList/ListMenu").Call("RemoveItem", this);
		_movementManager.Remove(this);
		QueueFree();
	}
}
