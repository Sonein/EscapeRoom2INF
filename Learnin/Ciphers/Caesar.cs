using System.Linq;
using System.Text;

namespace Learnin.Ciphers;

public class Caesar : ICipher
{
    private int _savedCode = 0;
    
    private bool CheckCode(string code)
    {
        return code.All(c => c is >= '0' and <= '9');
    }
    
    public string Encrypt(string input, string code)
    {
        if (!CheckCode(code) || code.Length == 0)
        {
            return input;
        }

        _savedCode = int.Parse(code);

        StringBuilder output = new StringBuilder();

        foreach (var c in input)
        {
            if (c is < 'a' or > 'z')
            {
                return input;
            }
            
            output.Append((char) ((c-97+_savedCode)%26+97));
        }

        return output.ToString();
    }

    public string Decrypt(string input)
    {
        StringBuilder output = new StringBuilder();

        foreach (var c in input)
        {
            if (c is < 'a' or > 'z')
            {
                return input;
            }

            int shift = (c - 97 - _savedCode) % 26;
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
        return "caesar";
    }
}