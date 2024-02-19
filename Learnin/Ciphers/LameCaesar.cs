using System.Text;

namespace Learnin.Ciphers;

public class LameCaesar : ICipher
{
    public string Encrypt(string input, string code)
    {
        StringBuilder output = new StringBuilder();
        
        foreach (var c in input)
        {
            if (c is < 'a' or > 'z')
            {
                return input;
            }

            if (c < 110)
            {
                output.Append((char) (2*(c-97)%26+97));
            }
            else
            {
                output.Append((char) (2*(c-97)%26+97+1));
            }
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
            
            if (c % 2 == 1)
            {
                output.Append((char) ((c-97)/2+97));
            }
            else
            {
                output.Append((char)((c-98)/2+97+13));
            }
        }
        
        return output.ToString();
    }

    public string Type()
    {
        return "caesarl";
    }
}