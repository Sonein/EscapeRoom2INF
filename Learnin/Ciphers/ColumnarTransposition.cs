using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learnin.Ciphers;

public class ColumnarTransposition : ICipher
{
    private string _code = "0";

    private bool HasAll(int max, List<char> list)
    {
        for (int i = 0; i < max; i++)
        {
            if (!list.Contains((char)('0' + i)))
            {
                return false;
            }
        }
        return true;
    }
    
    public string Encrypt(string input, string code)
    {
        StringBuilder deer = new StringBuilder();
        List<char> tempDeer = new List<char>();
        int max = 0;
        foreach (var c in code)
        {
            if (c is < '0' or > '9')
            {
                return input;
            }
            if (!tempDeer.Contains(c))
            {
                if (c - '0' > max)
                {
                    max = c - '0';
                }
                tempDeer.Add(c);
            }
            else
            {
                return input;
            }
        }
        if (HasAll(max, tempDeer))
        {
            foreach (var c in tempDeer)
            {
                deer.Append(c);
            }
            _code = deer.ToString();
        }
        else
        {
            return input;
        }

        List<StringBuilder> haruka = new List<StringBuilder>();
        StringBuilder[] harukaSorted = new StringBuilder[_code.Length];
        for (int i = 0; i < _code.Length; i++)
        {
            haruka.Add(new StringBuilder());
            harukaSorted[i] = new StringBuilder();
        }

        int karibu = 0;
        foreach (var c in input)
        {
            if (karibu == _code.Length)
            {
                karibu = 0;
            }
            haruka[karibu++].Append(c);
        }

        StringBuilder output = new StringBuilder();
        int pain = 0;
        foreach (var c in _code)
        {
            harukaSorted[c-'0'] = haruka[pain++];
        }

        foreach (var hehroo in harukaSorted)
        {
            output.Append(hehroo);
        }
        return output.ToString();
    }

    public string Decrypt(string input)
    {
        int haru = input.Length / _code.Length;
        int kari = input.Length % _code.Length;
        int where = 0;


        char[,] haruka = new char[_code.Length,haru+1];
        for (int i = 0; i < _code.Length; i++)
        {
            for (int j = 0; j < haru+1; j++)
            {
                haruka[i, j] = (char)0;
            }
        }
        List<int> karibu = _code.Select(c => c - '0').ToList();
        
        for (int i = 0; i < _code.Length; i++)
        {
            int karibuIndex = karibu.IndexOf(i);
            int karibuCol = 0;
            for (int j = 0; j < haru; j++)
            {
                haruka[karibuIndex, karibuCol++] = input[where++];
            }
            if (karibuIndex <= kari-1)
            {
                haruka[karibuIndex, karibuCol] = input[where++];
            }
        }

        StringBuilder output = new StringBuilder();
        for (int i = 0; i < haru+1; i++)
        {
            for (int j = 0; j < _code.Length; j++)
            {
                if (haruka[j, i] != 0)
                {
                    output.Append(haruka[j, i]);
                }
            }
        }
        return output.ToString();
    }

    public string Type()
    {
        return "coltrans";
    }
}