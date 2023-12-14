using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace Learnin;

public partial class DisconnectionMenu : MenuButton
{
	private System.Collections.Generic.Dictionary<string, int> _items;
	private PopupMenu _popupMenu;
	private bool _inGame;
	private int _id;
	private Node _toConnectTo;
	private string _toConnectToType;
	
	public override void _Ready()
	{
		_items = new System.Collections.Generic.Dictionary<string, int>();
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
			ObjectConnector.Disconnect(_toConnectTo, _toConnectToType, node);
			RemoveItem(name);
		}
	}

	private void AddItem(string x)
	{
		if (_items.ContainsKey(x))
		{
			return;
		}
		_items.Add(x, _id);
		_popupMenu.AddItem(x, _id++);
	}

	private void RemoveItem(string x)
	{
		int id = _items.GetValueOrDefault(x);
		int index = _popupMenu.GetItemIndex(id);
		_items.Remove(x);
		_popupMenu.RemoveItem(index);
	}

	private void ResetSelf(Node x, string type)
	{
		Array has = x.Call("GiveUpYourList").AsGodotArray();
		ClearSelf();
		_toConnectTo = x;
		_toConnectToType = type;
		foreach (string kirsch in has)
		{
			GD.Print(kirsch);
			AddItem(kirsch);
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
