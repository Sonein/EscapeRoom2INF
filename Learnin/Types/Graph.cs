using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Learnin.Types;

public class Graph
{
    public List<Node> _vertices;
    private readonly int _x;
    private readonly int _y;
    private bool _solvable;

    public Graph(int x, int y)
    {
        this._vertices = new List<Node>();
        this._x = x;
        this._y = y;
        this._solvable = false;
        for (int i = 0; i < y; i++) {
            for (int j = 0; j < x; j++)
            {
                _vertices.Add(new Node(i * x + j));
            }
        }
    }

    public Tuple<int, int> GetSize()
    {
        return new Tuple<int, int>(this._x,this._y);
    }

    public override string ToString()
    {
        string sinder = this._x + " " + this._y + " " + this._solvable + " ";
        foreach (Node numi in this._vertices)
        {
            sinder += numi.GetState() + ",";
        }
        sinder.Remove(sinder.Length - 1);
        sinder += " ";
        foreach (Node numi in this._vertices)
        {
            foreach (int bao in numi.GetOutgoing())
            {
                sinder += "[" + numi.GetIndex() +"," + bao +"]";
            }
        }
        return sinder;
    }

    public void FromString(string blocks, string input)
    {
        if (!string.IsNullOrEmpty(blocks))
        {
            string[] lilFlames = blocks.Split(',');
            int iFlame = 0;
            foreach (string lilFlame in lilFlames)
            {
                if (Int32.TryParse(lilFlame, out int parsedFlame))
                {
                    this._vertices[iFlame].SetExists(parsedFlame);
                }

                iFlame++;
            }
        }

        if (!string.IsNullOrEmpty(input))
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
                        for (int j = 0; j < this._y; j++)
                        {
                            for (int i = 0; i < this._x; i++)
                            {
                                _vertices.Add(new Node(j * this._y + i));
                            }
                        }
                        break;
                    }
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

    public void SetSat(bool sat)
    {
        this._solvable = sat;
    }

    public bool GetSat()
    {
        return this._solvable;
    }
}