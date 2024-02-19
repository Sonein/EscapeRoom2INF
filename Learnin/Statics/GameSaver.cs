using System.Collections.Generic;
using System.IO;
using System.Text;
using Godot;
using Godot.Collections;

namespace Learnin.Statics;

public class GameSaver
{
    private List<PolygonInfo> _polygonInfos = new();

    public void SaveState(List<Polygon2D> nodes)
    {
        _polygonInfos = new List<PolygonInfo>();
        foreach (var v in nodes)
        {
            string type = v.Call("GetShapeType").AsString();
            Array<string> connections = new Array<string>();
            if (!type.Equals("door") && !type.Equals("none"))
            {
                connections = v.Call("GiveUpYourList").AsGodotArray<string>().Duplicate();
            }
            PolygonInfo temp = new PolygonInfo(v.Name, type, v.Position, connections, (string)v.Call("GetSpecial"));
            _polygonInfos.Add(temp);
        }
    }

    public List<PolygonInfo> LoadState()
    {
        return _polygonInfos;
    }

    public void Save(string path, List<Polygon2D> nodes)
    {
        StreamWriter sw = new StreamWriter(path);
        SaveState(nodes);
        foreach (var polygonInfo in _polygonInfos)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(polygonInfo.Name).Append(' ');
            stringBuilder.Append(polygonInfo.Type).Append(' ');
            stringBuilder.Append(polygonInfo.Position.X).Append(' ').Append(polygonInfo.Position.Y).Append(' ');
            stringBuilder.Append(polygonInfo.Special).Append(' ');
            stringBuilder.Append('$');
            sw.WriteLine(stringBuilder);
        }
        sw.WriteLine('#');
        foreach (var polygonInfo in _polygonInfos)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(polygonInfo.Name).Append(' ');
            foreach (var node in polygonInfo.Connections)
            {
                stringBuilder.Append(node).Append(' ');
            }
            stringBuilder.Append('$');
            sw.WriteLine(stringBuilder);
        }
        sw.Close();
    }

    public List<PolygonInfo> Load(string path)
    {
        string file = File.ReadAllText(path);
        Scanner scanner = new Scanner(file);
        _polygonInfos = new List<PolygonInfo>();
        while (scanner.HasNext())
        {
            while (scanner.HasNext())
            {
                string name = scanner.Next();
                if (name.Equals("#"))
                {
                    break;
                }
                string type = scanner.Next();
                int posX = scanner.NextInt();
                int posY = scanner.NextInt();
                string special = "";
                string tempSpecial = scanner.Next();
                while (!tempSpecial.Equals("$"))
                {
                    special += (tempSpecial + " ");
                    tempSpecial = scanner.Next();
                }

                _polygonInfos.Add(new PolygonInfo(name, type, new Vector2(posX, posY), new Array<string>(), special));
            }

            while (scanner.HasNext())
            {
                string line = scanner.Next();
                if (!line.Equals("$"))
                {
                    Array<string> connections = new Array<string>();
                    while (scanner.HasNext())
                    {
                        string connector = scanner.Next();
                        if (connector.Equals("$"))
                        {
                            break;
                        }
                        connections.Add(connector);
                    }
                    for(int i = 0; i < _polygonInfos.Count; i++)
                    {
                        if (_polygonInfos[i].Name.Equals(line))
                        {
                            _polygonInfos[i] = new PolygonInfo(_polygonInfos[i].Name, _polygonInfos[i].Type,
                                _polygonInfos[i].Position, connections, _polygonInfos[i].Special);
                        }
                    }
                }
            }
        }
        scanner.Close();
        return _polygonInfos;
    }

}