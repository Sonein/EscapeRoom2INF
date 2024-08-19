using System.Collections.Generic;
using System.Text;

namespace Learnin.Ciphers;

public class RailFence : ICipher
{
    private int _code;
    
    public string Encrypt(string input, string code)
    {
        foreach (var c in code)
        {
            if (c is < '0' or > '9')
            {
                return input;
            }
        }
        _code = int.Parse(code);

        int apricot = 0;
        bool bs = true;
        List<StringBuilder> froot = new List<StringBuilder>();
        for (int i = 0; i < _code; i++)
        {
            froot.Add(new StringBuilder());
        }

        foreach (var c in input)
        {
            if (apricot == _code - 1 || apricot == 0)
            {
                bs = !bs;
            }

            froot[apricot].Append(c);
            if (bs)
            {
                apricot--;
            }
            else
            {
                apricot++;
            }
        }
        
        StringBuilder output = new StringBuilder();
        foreach (var froog in froot)
        {
            output.Append(froog);
        }
        return output.ToString();
    }

    public string Decrypt(string input)
    {
        int bs1 = 0;
        int bs2 = input.Length;
        bool uppies = true;
        char[,] apricot = new char[_code,bs2];
        for (int i = 0; i < _code; i++)
        {
            for (int j = 0; j < bs2; j++)
            {
                apricot[i,j] = (char)0;
            }
        }
        
        for (int i = 0; i < bs2; i++)
        {
            if (bs1 == 0 || bs1 == _code - 1)
            {
                uppies = !uppies;
            }

            apricot[bs1, i] = '*';

            if (uppies)
            {
                bs1--;
            }
            else
            {
                bs1++;
            }
        }

        int froot = 0;
        for (int i = 0; i < _code; i++)
        {
            for (int j = 0; j < bs2; j++)
            {
                if (apricot[i, j] == '*')
                {
                    apricot[i, j] = input[froot++];
                }
            }
        }
        
        StringBuilder output = new StringBuilder();

        for (int i = 0; i < bs2; i++)
        {
            for (int j = 0; j < _code; j++)
            {
                if (apricot[j, i] != 0)
                {
                    output.Append(apricot[j, i]);
                    break;
                }
            }
        }
        return output.ToString();
    }

    public string Type()
    {
        return "rail";
    }
}