using System;
using System.Collections.Generic;
using System.Text;

namespace Learnin.Ciphers;

public class Base648Bit : ICipher
{
    private List<char> _alphabet;
    private string _pad;

    public Base648Bit()
    {
        _alphabet = new List<char>();
        for (int i = 0; i < 26; i++)
        {
            _alphabet.Add((char)('A' + i));
        }
        for (int i = 0; i < 26; i++)
        {
            _alphabet.Add((char)('a' + i));
        }
        for (int i = 0; i < 10; i++)
        {
            _alphabet.Add((char)('0' + i));
        }
        _alphabet.Add('-');
        _alphabet.Add('_');
        _pad = "";
    }

    private string ToBits(int value, int bitLength)
    {
        StringBuilder sinderTheBuilder = new StringBuilder();
        int localValue = value;
        for (int i = bitLength - 1; i >= 0; i--)
        {
            if (localValue - Math.Pow(2, i) >= 0)
            {
                sinderTheBuilder.Append('1');
                localValue -= (int)Math.Pow(2, i);
            }
            else
            {
                sinderTheBuilder.Append('0');
            }
        }
        
        return sinderTheBuilder.ToString();
    }

    public string Encrypt(string input, string code)
    {
        StringBuilder bits = new StringBuilder();
        foreach (var c in input)
        {
            if (c >= 256)
            {
                return input;
            }
            string sinder = ToBits(c, 8);
            bits.Append(sinder);
        }
        if (bits.Length % 6 == 2)
        {
            _pad = "==";
            bits.Append("0000");
        } else if (bits.Length % 6 == 4)
        {
            _pad = "=";
            bits.Append("00");
        }

        StringBuilder output = new StringBuilder();
        for (int i = 0; i < bits.Length/6; i++)
        {
            StringBuilder localS = new StringBuilder();
            for (int j = 0; j < 6; j++)
            {
                localS.Append(bits[i * 6 + j]);
            }
            output.Append(_alphabet[Convert.ToInt32(localS.ToString(), 2)]);
        }
        output.Append(_pad);
        
        return output.ToString();
    }

    public string Decrypt(string input)
    {
        StringBuilder ollie = new StringBuilder();
        foreach (var c in input)
        {
            if (!_alphabet.Contains(c) && c != '=')
            {
                return input;
            }

            if(c == '=')
            {
                ollie.Remove(ollie.Length - 2, 2);
            }
            else
            {
                int toKureiji = _alphabet.IndexOf(c);
                ollie.Append(ToBits(toKureiji, 6));
                
            }
        }

        StringBuilder output = new StringBuilder();
        for (int i = 0; i < ollie.Length/8; i++)
        {
            StringBuilder localS = new StringBuilder();
            for (int j = 0; j < 8; j++)
            {
                localS.Append(ollie[i * 8 + j]);
            }

            output.Append((char)Convert.ToInt32(localS.ToString(), 2));
        }
        return output.ToString();
    }

    public string Type()
    {
        return "b648b";
    }
}