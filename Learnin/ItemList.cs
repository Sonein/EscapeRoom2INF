using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Learnin;

public partial class ItemList : MenuButton
{
	private System.Collections.Generic.Dictionary<Polygon2D, int> _items;

	private PopupMenu _popupMenu;

	private int _id;

	private bool _inGame;
	
	public override void _Ready()
	{
		_items = new System.Collections.Generic.Dictionary<Polygon2D, int>();
		_popupMenu = this.GetPopup();
		Callable callable = new Callable(this, nameof(OnMenuItemSelected));
		_popupMenu.Connect("id_pressed", callable);
		_id = 0;
	}
	
	public override void _Process(double delta)
	{
	}

	public void AddItem(Polygon2D x)
	{
		_items.Add(x, _id);
		_popupMenu.AddItem(x.Name, _id++);
	}

	public void RemoveItem(Polygon2D x)
	{
		int id = _items.GetValueOrDefault(x);
		int index = _popupMenu.GetItemIndex(id);
		_items.Remove(x);
		_popupMenu.RemoveItem(index);
	}
	
	private void OnMenuItemSelected(int id)
	{
		if (!_inGame)
		{
			int index = _popupMenu.GetItemIndex(id);
			string name = _popupMenu.GetItemText(index);
			GetNode<Node>("/root/Main/" + name).Call("SetInternals", "select");
		}
	}
	
	public void SetInternals(string x)
	{
		switch (x)
		{
			case "play":
				_inGame = !_inGame;
				if (!_inGame)
				{
					ClearSelf();
				}

				break;
		}
	}

	public Array GetNodes()
	{
		var temp = new Array();
		foreach (var x in _items.Keys)
		{
			temp.Add(x);
		}
		return temp;
	}
	
	private void ClearSelf()
	{
		_items.Clear();
		_popupMenu.Clear();
		_id = 0;
	}
}
