using Godot;
using Godot.Collections;

namespace Learnin;

public struct PolygonInfo
{
    public PolygonInfo(string name, string type, Vector2 position, Array<string> array)
    {
        Name = name;
        Type = type;
        Position = position;
        Connections = array.Duplicate();
    }

    public string Name { get; }
    public string Type { get; }
    public Vector2 Position { get; }
    public Array<string> Connections { get; set; }
}