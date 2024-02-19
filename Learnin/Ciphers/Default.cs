namespace Learnin.Ciphers;

public class Default : ICipher
{
    public string Encrypt(string input, string code)
    {
        return input;
    }

    public string Decrypt(string input)
    {
        return input;
    }

    public string Type()
    {
        return "def";
    }
}