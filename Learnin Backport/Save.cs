using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using Learnin.Statics;

namespace Learnin;

public class Save : Button
{
	private bool _inGame;
	private GameSaver _gameSaver;
	private string _path;
	private TextEdit _textEdit;

	public override void _Ready()
	{
		_gameSaver = new GameSaver();
		_textEdit = (TextEdit)GetChildren()[0];
	}


	public override void _Process(float delta)
	{
	}
	
	private void OnSaveButtonDown()
	{
		if (!_inGame)
		{
			var nodes = (Array<Polygon2D>)GetNode<Node>("/root/Main/Menu/ItemList/ListMenu").Call("GetNodes");
			List<Polygon2D> nodesFr = nodes.ToList();
			_gameSaver.Save(_path, nodesFr);
		}
	}

	private void OnSavePathTextChanged()
	{
		_path = _textEdit.Text;
	}
	
	private void SetInternals(string x)
	{
		switch (x)
		{
			case "play":
				_inGame = !_inGame;
				break;
		}
	}
}
