using Godot;
using System;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace Learnin;
	
public class ItemList : MenuButton
{
	
	/* Needs font_size = 45 and 7 items. */
	private System.Collections.Generic.Dictionary<Polygon2D, int> _items;

	private PopupMenu _popupMenu;

	private int _id;

	private bool _inGame;
	
	public override void _Ready()
	{
		_items = new System.Collections.Generic.Dictionary<Polygon2D, int>();
		_popupMenu = GetPopup();
		_popupMenu.Connect("id_pressed", this, "OnMenuItemSelected");
		_id = 0;
		if (this.GetFont("font", "") is DynamicFont font)
		{
			font.Size = 45;
			this.AddFontOverride("font", font);
		}
	}

	public override void _Process(float delta)
	{
	}
	
	public void AddItem(Polygon2D x)
	{
		_items.Add(x, _id);
		_popupMenu.AddItem(x.Name, _id++);
	}

	public void RemoveItem(Polygon2D x)
	{
		int id;
		_items.TryGetValue(x, out id);
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

	public Array<Polygon2D> GetNodes()
	{
		Array<Polygon2D> temp = new Array<Polygon2D>();
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
