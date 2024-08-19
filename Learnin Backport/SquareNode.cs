using Godot;
using Learnin.Statics;

namespace Learnin;

public class SquareNode : Polygon2D
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

	public override void _Process(float delta)
	{
		if (_isDragging)
		{
			_isDragging = _movementManager.CanMove(this);
			FollowIronMouse();
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		//TODO safety pri davani kodu
		if (@event is InputEventKey eventKey && eventKey.Pressed && _isSelected)
		{
			switch (eventKey.Scancode)
			{
				case 68:
					SigKill();
					break;
			}
		}
	}
	
	private void OnArea2DInputEvent(object viewport, object @event, int shape_idx)
	{
		if (!_inGame)
		{
			if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == 1)
			{
				_isDragging = _movementManager.StartMove(@event as InputEventMouseButton, GetGlobalMousePosition(), Position, this);
				if (_isDragging)
				{
					_ironMouseOffset = _movementManager.IronmouseOffset;
				}
			}
			else if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == 2)
			{
				if ((@event as InputEventMouseButton).IsPressed())
				{
					SetInternals("select");
				}
			}
			if (@event is InputEventMouseButton bb && bb.Doubleclick)
			{
				GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("ClearSelf");	
				GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ClearSelf");
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
