using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Learnin.Statics;

namespace Learnin;

public class Key : Polygon2D
{
	private bool _isSelected;
	private bool _isDragging;
	private bool _inGame;
	private bool _unlocked;
	private Vector2 _ironMouseOffset;
	private System.Collections.Generic.Dictionary<string, Vector2[]> _locks;
	private MovementManager _movementManager;

	public override void _Ready()
	{
		_locks = new System.Collections.Generic.Dictionary<string, Vector2[]>();
		_unlocked = true;
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

		if (_inGame)
		{
			if (_unlocked)
			{
				GetNode<MenuButton>("/root/Main/Menu/ItemList/ListMenu").Call("RemoveItem", this);
				_movementManager.Remove(this);
				QueueFree();
			}
			else
			{
				FindingLock();
			}
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
			GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("GenerateOptions", this, "lock");
			GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ResetSelf", this, "key");
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
				if (_inGame)
				{
					var i = new List<string>(_locks.Keys);
					foreach (var key in i)
					{
						_locks[key] = new[]
						{
							GetNode<Polygon2D>("/root/Main/" + key).Position,
							GetNode<Polygon2D>("/root/Main/" + key).Position + new Vector2(175, 175)
						};
					}
				}
				break;
		}
	}
	
	private void AddLock(string boi)
	{
		if (_locks.ContainsKey(boi)) return;
		Vector2[] temp = new []{new Vector2(), new Vector2()};
		//GD.Print(temp[0] + " " + temp[1]);
		_unlocked = false;
		_locks.Add(boi, temp);
	}

	private void RemoveLock(string boi)
	{
		if (!_locks.ContainsKey(boi)) return;
		_locks.Remove(boi);
	}

	private void FindingLock()
	{
		string thePippa = "none";
		foreach (var pippa in _locks)
		{
			//GD.Print(Position.X + "," + Position.Y + "/" + pippa.Value[0].X + "," + pippa.Value[0].Y + "/" + pippa.Value[1].X + "," + pippa.Value[1].Y);
			if ((Position.x >= pippa.Value[0].x && Position.y >= pippa.Value[0].y)
				&& (Position.x <= pippa.Value[1].x && Position.y <= pippa.Value[1].y))
			{
				GetNode<Polygon2D>("/root/Main/" + pippa.Key).Call("RemoveKey", this);
				thePippa = pippa.Key;
				break;
			}
		}
		if (!thePippa.Equals("none"))
		{
			_locks.Remove(thePippa);
		}

		if (!_locks.Any())
		{
			_unlocked = true;
		}
	}

	private Array<string> GiveUpYourList()
	{
		return new Array<string>(_locks.Keys);
	}
	
	private string GetShapeType()
	{
		return "key";
	}

	private void SigKill()
	{
		GetNode<MenuButton>("/root/Main/Menu/ItemList/ListMenu").Call("RemoveItem", this);
		GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("ClearSelf");
		GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ClearSelf");
		foreach (var boi in _locks)
		{
			GetNode<Polygon2D>("/root/Main/" + boi.Key).Call("RemoveKey", this);
		}
		_movementManager.Remove(this);
		QueueFree();
	}
}
