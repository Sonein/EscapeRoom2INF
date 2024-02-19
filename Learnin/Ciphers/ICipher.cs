namespace Learnin.Ciphers;

public interface ICipher
{
    string Encrypt(string input, string code);
    string Decrypt(string input);
    string Type();
}