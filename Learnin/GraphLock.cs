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
	private Stack<int> _way;
	private int _start;
	private int _last;
	
	public override void _Ready()
	{
		if (_graph == null)
		{
			_graph = new Graph(0, 0);
		}
		_board = (Polygon2D)GetChildren()[4];
		_board.Visible = false;
		_doors = new List<string>();
		_dimension = (TextEdit)GetChildren()[1];
		_movementManager = MovementManager.Instance;
		_movementManager.Add(this);
		_way = new Stack<int>();
		_last = -1;
		_start = -1;
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
					if (pup1 > 10 || pup2 > 10)
					{
						return;
					}
					_graph = new Graph(pup1, pup2);
					//TODO add smol shits to board
					foreach (Godot.Node gn in _board.GetChildren())
					{
						gn.Free();
					}
					_board.Polygon = new[] { new Vector2(200, 0), new Vector2(220 + pup1*60, 0), new Vector2(220 + pup1*60, 20 + pup2*60), new Vector2(200, 20 + pup2*60) };
					for (int y = 0; y < pup2; y++)
					{
						for (int x = 0; x < pup1; x++)
						{
							string name = "Pyro" + x + y;
							Polygon2D pyropup = ObjectCreator.Create(name, "smol", new Vector2(210 + x * 60, 10 + y * 60), (y * pup1 + x).ToString());
							_board.AddChild(pyropup);
						}
					}
				}
			}
		}
	}


	private void _on_set_button_down()
	{
		//TODO parse board and check hamilton
		if (!_inGame)
		{
			int pup1 = this._graph.GetSize().Item1;
			int pup2 = this._graph.GetSize().Item2;
			for (int x = 0; x < pup1; x++)
			{
				for (int y = 0; y < pup2; y++)
				{
					int state = (int)this._board.GetChildren()[y * pup1 + x].Call("GetState");
					this._graph._vertices[y * pup1 + x].SetExists(state);
					if (state != 1)
					{
						if (x > 0)
						{
							int s = (int)this._board.GetChildren()[y * pup1 + x - 1].Call("GetState");
							if (s != 1)
							{
								this._graph._vertices[y * pup1 + x].AddOutgoing(y * pup1 + x - 1);
							}
						}

						if (x < pup1 - 1)
						{
							int s = (int)this._board.GetChildren()[y * pup1 + x + 1].Call("GetState");
							if (s != 1)
							{
								this._graph._vertices[y * pup1 + x].AddOutgoing(y * pup1 + x + 1);
							}
						}

						if (y > 0)
						{
							int s = (int)this._board.GetChildren()[(y - 1) * pup1 + x].Call("GetState");
							if (s != 1)
							{
								this._graph._vertices[y * pup1 + x].AddOutgoing((y - 1) * pup1 + x);
							}
						}

						if (y < pup2 - 1)
						{
							int s = (int)this._board.GetChildren()[(y + 1) * pup1 + x].Call("GetState");
							if (s != 1)
							{
								this._graph._vertices[y * pup1 + x].AddOutgoing((y + 1) * pup1 + x);
							}
						}
					}
				}
			}

			_unlocked = !HamiltonianChecker.HasHamilton(this._graph);
		}
		else
		{
			if (_graph._vertices[_last].GetOutgoing().Contains(_start))
			{
				if (HamiltonianChecker.IsHamilton(_graph, new List<int>(_way)))
				{
					_unlocked = true;
				}
			}
			//TODO cesticky
		}
	}

	private bool CanAdd(int node)
	{
		if (_last == -1)
		{
			_start = node;
			_way.Push(node);
			_last = node;
			return true;
		}
		if (_graph._vertices[_last].GetOutgoing().Contains(node))
		{
			_way.Push(node);
			_last = node;
			return true;
		}
		else
		{
			return false;
		}
	}

	private bool CanRemove(int node)
	{
		if (_last == node)
		{
			_way.Pop();
			if (_way.Count > 0)
			{
				_last = _way.Peek();
			}
			else
			{
				_start = -1;
				_last = -1;
			}
			return true;
		}
		else
		{
			return false;
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
				foreach (Godot.Node fart in _board.GetChildren())
				{
					fart.Call("Gamin");
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
		GD.Print(special);
		Scanner scanner = new Scanner(special);
		int pup1 = scanner.NextInt();
		int pup2 = scanner.NextInt();
		string states = scanner.Next();
		string edges = scanner.Next();
		_graph = new Graph(pup1, pup2);
		_graph.FromString(states, edges);
		//TODO restore board
		_board = (Polygon2D)GetChildren()[4];
		foreach (Godot.Node gn in _board.GetChildren())
		{
			gn.Free();
		}
		_board.Polygon = new[] { new Vector2(200, 0), new Vector2(220 + pup1*60, 0), new Vector2(220 + pup1*60, 20 + pup2*60), new Vector2(200, 20 + pup2*60) };
		for (int y = 0; y < pup2; y++)
		{
			for (int x = 0; x < pup1; x++)
			{
				string name = "Pyro" + x + y;
				Polygon2D pyropup = ObjectCreator.Create(name, "smol", new Vector2(210 + x * 60, 10 + y * 60), (y * pup1 + x).ToString());
				_board.AddChild(pyropup);
				pyropup.Call("SetState", _graph._vertices[y * pup1 + x].GetState());
			}
		}
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
