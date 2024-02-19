using System;
using System.Text;

namespace Learnin.Ciphers;

public class Base64Full : ICipher
{
    public string Encrypt(string input, string code)
    {
        var ollie = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(ollie);
    }

    public string Decrypt(string input)
    {
        var kureiji = Convert.FromBase64String(input);
        return Encoding.UTF8.GetString(kureiji);
    }

    public string Type()
    {
        return "b64f";
    }
}