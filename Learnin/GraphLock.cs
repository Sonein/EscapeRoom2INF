using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Learnin.Statics;
using Learnin.Types;

namespace Learnin;

public partial class GraphLock : Polygon2D
{
	private Graph _graph;
	private TextEdit _dimension;
	private bool _isSelected;
	private bool _isDragging;
	private bool _inGame;
	private bool _unlocked;
	private Vector2 _ironMouseOffset;
	private List<string> _doors;
	private MovementManager _movementManager;
	private Polygon2D _board;
	
	public override void _Ready()
	{
		_graph = new Graph(0, 0);
		_board = (Polygon2D)GetChildren()[4];
		_board.Visible = false;
		_doors = new List<string>();
		_dimension = (TextEdit)GetChildren()[1];
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
		
		if (_inGame && _unlocked)
		{
			foreach (var door in _doors)
			{
				GetNode<Polygon2D>("/root/Main/" + door).Call("RemoveLock", this);
			}
			GetNode<MenuButton>("/root/Main/Menu/ItemList/ListMenu").Call("RemoveItem", this);
			_movementManager.Remove(this);
			QueueFree();
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
	
	private void _on_area_2d_input_event(Godot.Node viewport, InputEvent @event, long shape_idx)
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
			if (@event is InputEventMouseButton { DoubleClick: true })
			{
				GetNode<Godot.Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("GenerateOptions", this, "door");
				GetNode<Godot.Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ResetSelf", this, "graph");
			}
		}
	}


	private void _on_submit_button_down()
	{
		if (!_inGame)
		{
			string sinder = _dimension.Text;
			Scanner scanner = new Scanner(sinder);
			if (scanner.HasNextInt())
			{
				int pup1 = scanner.NextInt();
				if (scanner.HasNextInt())
				{
					int pup2 = scanner.NextInt();
					_graph = new Graph(pup1, pup2);
					//TODO add smol shits to board
				}
			}
		}
	}


	private void _on_set_button_down()
	{
		//TODO parse board and check hamilton
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
				_board.Visible = _isSelected;
				break;
			case "drag":
				_isDragging = !_isDragging;
				break;
			case "play":
				_inGame = !_inGame;
				_dimension.Visible = false;
				_board.Visible = false;
				((Button)GetChildren()[2]).Visible = false;
				if (_inGame)
				{
					_dimension.Clear();
					if (_graph.GetSize().Item1 == 0 || _graph.GetSize().Item2 == 0)
					{
						_unlocked = true;
					}
					_board.Visible = true;
				}
				break;
		}
	}
	
	private void AddDoor(string door)
	{
		if (_doors.Contains(door)) return;
		_doors.Add(door);
	}

	private void RemoveDoor(string door)
	{
		if (!_doors.Contains(door)) return;
		_doors.Remove(door);
	}
	
	private Array<string> GiveUpYourList()
	{
		return new Array<string>(_doors);
	}
	
	private string GetShapeType()
	{
		return "graph";
	}

	private string GetSpecial()
	{
		return _graph.ToString();
	}

	private void SetSpecial(string special)
	{
		Scanner scanner = new Scanner(special);
		int x = scanner.NextInt();
		int y = scanner.NextInt();
		string states = scanner.Next();
		string edges = scanner.Next();
		_graph = new Graph(x, y);
		_graph.FromString(states, edges);
	}

	private void SigKill()
	{
		GetNode<MenuButton>("/root/Main/Menu/ItemList/ListMenu").Call("RemoveItem", this);
		GetNode<Godot.Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("ClearSelf");
		GetNode<Godot.Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ClearSelf");
		foreach (var door in _doors)
		{
			GetNode<Polygon2D>("/root/Main/" + door).Call("RemoveLock", this);
		}
		_movementManager.Remove(this);
		QueueFree();
	}
}
