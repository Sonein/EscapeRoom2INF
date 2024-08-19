using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learnin.Ciphers;

public class CyclicPermutation : ICipher
{
    private List<int> _shiftedAlphabet = new List<int>();

    private bool ParseCode(string code)
    {
        if (code.Length == 0)
        {
            return false;
        }

        foreach (var c in code)
        {
            if (c is < 'a' or > 'z' && c != ',')
            {
                return false;
            }
        }

        char first = code[0];
        char before = code[0];
        int size = code.Length;

        for (int i = 1; i < size; i++)
        {
            if (first == '#')
            {
                first = code[i];
                before = code[i];
                continue;
            }
            if (first == ',')
            {
                return false;
            }

            if (_shiftedAlphabet[before - 'a'] != -1)
            {
                return false;
            }

            if (code[i] == ',')
            {
                _shiftedAlphabet[before - 'a'] = first - 'a';
                first = '#';
            }
            else
            {
                _shiftedAlphabet[before - 'a'] = code[i] - 'a';
                before = code[i];
            }
        }

        if (code[size - 1] != ',')
        {
            _shiftedAlphabet[code[size - 1] - 'a'] = first - 'a';
        }

        if (_shiftedAlphabet.Contains(-1))
        {
            return false;
        }

        return true;
    }
    
    public string Encrypt(string input, string code)
    {
        _shiftedAlphabet = new List<int>();
        for (int i = 0; i < 26; i++)
        {
            _shiftedAlphabet.Add(-1);
        }

        if (!ParseCode(code))
        {
            return input;
        }

        StringBuilder output = new StringBuilder();
        foreach (var c in input)
        {
            output.Append((char) ('a' + _shiftedAlphabet[c - 'a']));
        }
        return output.ToString();
    }

    public string Decrypt(string input)
    {
        if (_shiftedAlphabet.Contains(-1) || !_shiftedAlphabet.Any())
        {
            return input;
        }

        StringBuilder output = new StringBuilder();

        foreach (var c in input)
        {
            if (c is < 'a' or > 'z')
            {
                return input;
            }

            output.Append((char) ('a' + _shiftedAlphabet.IndexOf(c - 'a')));
        }
        
        return output.ToString();
    }

    public string Type()
    {
        return "cycle";
    }
}