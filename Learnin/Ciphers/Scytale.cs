using System.Collections.Generic;
using System.Text;

namespace Learnin.Ciphers;

public class Scytale : ICipher
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
        
        char[] zen = new char[input.Length];
        int treya = 0;
        for (int i = 0; i < _code; i++)
        {
            int j = i;
            while (j < input.Length)
            {
                zen[j] = input[treya++];
                j += _code;
            }
        }
        
        StringBuilder output = new StringBuilder();
        foreach (var c in zen)
        {
            output.Append(c);
        }
        return output.ToString();
    }

    public string Decrypt(string input)
    {
        List<StringBuilder> zen = new List<StringBuilder>();
        for (int i = 0; i < _code; i++)
        {
            zen.Add(new StringBuilder());
        }
        int treya = 0;
        foreach (var c in input)
        {
            zen[treya % _code].Append(c);
            treya++;
        }
        
        StringBuilder output = new StringBuilder();
        foreach (var zentreya in zen)
        {
            foreach (var c in zentreya.ToString())
            {
                output.Append(c);
            }
        }
        
        return output.ToString();
    }

    public string Type()
    {
        return "scytale";
    }
}