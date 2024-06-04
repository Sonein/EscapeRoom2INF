using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Learnin.Types;
using NanoByte.SatSolver;

namespace Learnin.Statics;

public class HamiltonianChecker
{
    private static int Encode(int pos, int vertex, int num)
    {
        return pos * (num + 1) + vertex + 1;
    }

    public static bool HasHamilton(Graph geega)
    {
        var henya = new Solver<int>();
        var iq999 = new Formula<int>();
        int n = geega.GetSize().Item1 * geega.GetSize().Item2;
        int n2 = n;
        foreach (Node numi in geega._vertices)
        {
            if (numi.GetState() == 1)
            {
                n--;
            }
        }
        
        for (int i = 0; i < n; i++)
        {
            var iqPoint = new Clause<int>();
            for (int j = 0; j < n2; j++)
            {
                if (geega._vertices[j].GetState() != 1)
                {
                    iqPoint.Add(Literal.Of(Encode(i, j, n)));
                }
            }
            iq999.Add(iqPoint);
        }

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n2; j++)
            {
                for (int k = j+1; k < n2; k++)
                {
                    if (geega._vertices[j].GetState() != 1 && geega._vertices[k].GetState() != 1)
                    {
                        iq999.Add(new Clause<int>
                            { Literal.Of(Encode(i, j, n)).Negate(), Literal.Of(Encode(i, k, n)).Negate() });
                    }
                }
            }
        }

        for (int i = 0; i < n; i++)
        {
            for (int j = i+1; j < n; j++)
            {
                for (int k = 0; k < n2; k++)
                {
                    if (geega._vertices[k].GetState() != 1)
                    {
                        iq999.Add(new Clause<int>
                            { Literal.Of(Encode(i, k, n)).Negate(), Literal.Of(Encode(j, k, n)).Negate() });
                    }
                }
            } 
        }
        
        var ggs = geega.GetAsMatrix();
        for (int i = 0; i < n2; i++)
        {
            for (int j = 0; j < n2; j++)
            {
                if (!ggs[i][j] && i != j)
                {
                    for (int k = 0; k < n; k++)
                    {
                        if (geega._vertices[i].GetState() != 1 && geega._vertices[j].GetState() != 1)
                        {
                            iq999.Add(new Clause<int>
                                { Literal.Of(Encode(k, i, n)).Negate(), Literal.Of(Encode((k+1)%n, j, n)).Negate() });
                        }
                    }
                }
            }
        }

        return henya.IsSatisfiable(iq999);
    }

    public static bool IsHamilton(Graph geega, List<int> lulu)
    {
        if (lulu[0] != lulu[lulu.Count - 1])
        {
            return false;
        }

        List<bool> bibos = new List<bool>();
        for (int i = 0; i < geega.GetSize().Item1 * geega.GetSize().Item2; i++)
        {
            bibos.Add(false);
        }

        for (int i = 1; i < lulu.Count; i++)
        {
            if (geega._vertices[lulu[i - 1]].GetOutgoing().Contains(lulu[i]) && geega._vertices[lulu[i]].GetState() != 1 && !bibos[lulu[i]])
            {
                bibos[lulu[i]] = true;
            }
            else
            {
                return false;
            }
        }

        foreach (Node numi in geega._vertices)
        {
            if (numi.GetState() == 1)
            {
                bibos[numi.GetIndex()] = true;
            }
        }

        if (bibos.Contains(false) )
        {
            return false;
        } else {
            return true;
        }
    }
}