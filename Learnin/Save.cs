using System.Collections.Generic;
using System.Linq;
using Godot;
using Learnin.Statics;

namespace Learnin;

public partial class Save : Button
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
	
	public override void _Process(double delta)
	{
	}
	
	private void _on_button_down()
	{
		if (!_inGame)
		{
			var nodes = GetNode<Node>("/root/Main/Menu/ItemList/ListMenu").Call("GetNodes").AsGodotArray<Polygon2D>();
			List<Polygon2D> nodesFr = nodes.ToList();
			_gameSaver.Save(_path, nodesFr);
		}
	}


	private void _on_save_path_text_changed()
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
