using Godot;

namespace Learnin;

public partial class ConnectionMenu : MenuButton
{
	private System.Collections.Generic.Dictionary<Node, int> _items;
	private PopupMenu _popupMenu;
	private bool _inGame;
	private int _id;
	private Node _toConnectTo;
	private string _toConnectToType;
	
	public override void _Ready()
	{
		_items = new System.Collections.Generic.Dictionary<Node, int>();
		_popupMenu = this.GetPopup();
		Callable callable = new Callable(this, nameof(OnMenuItemSelected));
		_popupMenu.Connect("id_pressed", callable);
		_id = 0;
	}
	
	public override void _Process(double delta)
	{
	}
	
	private void OnMenuItemSelected(int id)
	{
		if (!_inGame)
		{
			int index = _popupMenu.GetItemIndex(id);
			string name = _popupMenu.GetItemText(index);
			Node node = GetNode<Node>("/root/Main/" + name);
			ObjectConnector.Connect(_toConnectTo, _toConnectToType, node);
			GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("AddItem", name);
		}
	}

	private void GenerateOptions(Node node, string type)
	{
		ClearSelf();
		_toConnectTo = node;
		_toConnectToType = (string)node.Call("GetShapeType");
		var nodes = GetNode<Node>("/root/Main/Menu/ItemList/ListMenu").Call("GetNodes").AsGodotArray();
		GD.Print(nodes);
		foreach (Node x in nodes)
		{
			string nodeType = (string)x.Call("GetShapeType");
			GD.Print(nodeType + " " + _toConnectToType);
			if (nodeType.Equals(type))
			{
				_items.Add(x, _id);
				_popupMenu.AddItem(x.Name, _id++);
			}
		}
	}

	private void SetInternals(string x)
	{
		switch (x)
		{
			case "play":
				_inGame = !_inGame;
				ClearSelf();
				break;
		}
	}

	private void ClearSelf()
	{
		_toConnectTo = null;
		_items.Clear();
		_popupMenu.Clear();
		_id = 0;
	}
}
