namespace Learnin.Statics;

public static class Tooltip
{
    public static readonly string Default = "Allowed ciphers are: b64f, b648b, " +
                                            "caesar, caesarl," +
                                            "coltrans, cycle, " +
                                            "rail, scytale, vigenere. " +
                                            "After typing in the cipher into the cipher type box it's syntax will show here.";

    public static readonly string B64F = "Standard Base64 encryption using UTF-8 encoding. Code is not necessary.";
    public static readonly string B648B = "Standard Base64 encryption allowing only ASCII characters in plaintext. Code is not necessary. Wrong input will be saved untouched.";
    public static readonly string Caesar = "Standard Caesar cipher. The plaintext should contain only letters of small English alphabet." +
                                           "Code allows only numbers. Wrong input will be saved untouched.";
    public static readonly string CaesarL = "Caesar cipher that shifts each character by itself. The plaintext should contain only letters of small English alphabet." +
                                            "Code is not necessary. Wrong input will be saved untouched.";
    public static readonly string ColTrans = "Standard Columnar Transposition cipher allowing up to ten columns. The columns in the code are numbered from 0 to 9." +
                                             "The code must contain all numbers between min and max. Wrong input will be saved untouched.";
    public static readonly string Cycle = "A Cyclic permutation cipher. Syntax is that of cyclic notation of permutations in algebra." +
                                          "Each cycle is separated by a comma and all letter must be written. Plaintext allows only small letters of EN alphabet. " +
                                          "Wrong input will be saved untouched.";
    public static readonly string Rail = "Standard Rail Fence cipher. Code represents the number of rows. Wrong input will be saved untouched.";
    public static readonly string Scytale = "Standard Scytale cipher. Code represents the number of rows. Wrong input will be saved untouched.";
    public static readonly string Vigenere = "Standard Vigenere cipher allowing only small letters of EN alphabet. Wrong input will be saved untouched.";
}