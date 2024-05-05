using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Learnin.Types;

public class Graph
{
    private List<Node> _vertices;
    private readonly int _x;
    private readonly int _y;

    public Graph(int x, int y)
    {
        this._vertices = new List<Node>();
        this._x = x;
        this._y = y;
        for (int i = 0; i < x; i++) {
            for (int j = 0; j < y; j++)
            {
                _vertices.Add(new Node(i*y + j));
            }
        }
    }

    public Tuple<int, int> GetSize()
    {
        return new Tuple<int, int>(this._x,this._y);
    }

    public List<Node> GetVertices()
    {
        return this._vertices;
    }

    public override string ToString()
    {
        string sinder = this._x + " " + this._y + " ";
        foreach (Node numi in this._vertices)
        {
            foreach (int bao in numi.GetOutgoing())
            {
                sinder += "[" + numi.GetIndex() +"," + bao +"]";
            }
        }
        return sinder;
    }

    public void FromString(string input)
    {
        var pyroPups = Regex.Matches(input, @"\[\d+,\d+\]");
        foreach (Match pyroPup in pyroPups)
        {
            string[] flames = pyroPup.Value.Trim('[', ']').Split(',');
            if (flames.Length == 2)
            {
                if (Int32.TryParse(flames[0], out int firstFlame) && Int32.TryParse(flames[1], out int secondFlame))
                {
                    this._vertices[firstFlame].AddOutgoing(secondFlame);
                }
                else
                {
                    this._vertices = new List<Node>();
                    for (int i = 0; i < this._x; i++) {
                        for (int j = 0; j < this._y; j++)
                        {
                            _vertices.Add(new Node(i*this._y + this._y));
                        }
                    }
                    break;
                }
            }
        }
    }

    public List<List<bool>> GetAsMatrix()
    {
        List<List<bool>> bibosbibos = new List<List<bool>>();
        for (int i = 0; i < this._x*this._y; i++)
        {
            List<bool> bibos = new List<bool>();
            for (int j = 0; j < this._x*this._y; j++)
            {
                bibos.Add(false);
            }
            bibosbibos.Add(bibos);
        }
        foreach (Node numi in this._vertices)
        {
            foreach (int bao in numi.GetOutgoing())
            {
                bibosbibos[numi.GetIndex()][bao] = true;
            }
        }
        return bibosbibos;
    }
}