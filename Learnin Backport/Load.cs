using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using Learnin.Statics;

namespace Learnin;

public class Load : Button
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
	
	private void OnLoadButtonDown()
	{
		if (!_inGame)
		{
			var node = GetNode<Node>("/root/Main/Menu/ItemList/ListMenu");
			var nodes = Caster.CastToArrayPoly2D(node.Call("GetNodes"));
			foreach (var variableNode in nodes)
			{
				variableNode.Free();
			}
			nodes.Clear();
			node.Call("ClearSelf");
			CallDeferred("Rebuild", node);
		}
	}


	private void OnLoadPathTextChanged()
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

	private void Rebuild(Node node)
	{
		List<PolygonInfo> polygonInfos = _gameSaver.Load(_path);
		foreach (var polygonInfo in polygonInfos)
		{
			Polygon2D tempPolygon = ObjectCreator.Create(polygonInfo.Name, polygonInfo.Type, polygonInfo.Position, polygonInfo.Special);
			GetNode<Node>("/root/Main").AddChild(tempPolygon);
			node.Call("AddItem", tempPolygon);
		}

		foreach (var polygonInfo in polygonInfos)
		{
			Node caller = GetNode<Node>("/root/Main/" + polygonInfo.Name);
			string callerType = polygonInfo.Type;
			foreach (var calledName in polygonInfo.Connections)
			{
				Node called = GetNode<Node>("/root/Main/" + calledName);
				ObjectConnector.Connect(caller, callerType, called);
			}
		}
	}
}
