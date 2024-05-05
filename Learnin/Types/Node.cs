using System.Collections.Generic;

namespace Learnin.Types;

public class Node
{
    private readonly int _index;
    private int _state;
    private readonly List<int> _outgoing;

    public Node(int index)
    {
        this._index = index;
        this._state = 0;
        this._outgoing = new List<int>();
    }

    public int GetIndex()
    {
        return this._index;
    }

    public int GetState()
    {
        return this._state;
    }

    public void SetExists(int ina)
    {
        this._state = ina;
    }

    public void AddOutgoing(int vertex)
    {
        this._outgoing.Add(vertex);
    }

    public List<int> GetOutgoing()
    {
        return this._outgoing;
    }
}