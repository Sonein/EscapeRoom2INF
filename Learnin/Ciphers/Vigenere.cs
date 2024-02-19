using System.Text;

namespace Learnin.Ciphers;

public class Vigenere : ICipher
{
    private string _savedCode = "";


    public string Encrypt(string input, string code)
    {
        if (code.Length == 0)
        {
            return input;
        }
        _savedCode = code;
        int size = code.Length;

        StringBuilder output = new StringBuilder();

        int i = 0;
        foreach (var c in input)
        {
            if (c is < 'a' or > 'z')
            {
                return input;
            }

            if (i >= size)
            {
                i = 0;
            }
            
            int shift = (c - 97 + _savedCode[i++] - 97) % 26;
            if (shift < 0)
            {
                shift += 26;
            }
            output.Append((char) (shift+97));
        }

        return output.ToString();
    }

    public string Decrypt(string input)
    {
        if (_savedCode.Length == 0)
        {
            return input;
        }
        StringBuilder output = new StringBuilder();
        int size = _savedCode.Length;

        int i = 0;
        foreach (var c in input)
        {
            if (c is < 'a' or > 'z')
            {
                return input;
            }
            
            if (i >= size)
            {
                i = 0;
            }
            
            int shift = (c - 97 - _savedCode[i++] + 97) % 26;
            if (shift < 0)
            {
                shift += 26;
            }
            output.Append((char) (shift+97));
        }

        return output.ToString();
    }

    public string Type()
    {
        return "vigenere";
    }
}