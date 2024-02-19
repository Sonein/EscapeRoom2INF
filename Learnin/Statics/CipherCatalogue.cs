using Learnin.Ciphers;

namespace Learnin.Statics;

public class CipherCatalogue
{
    public static ICipher GetCipher(string type)
    {
        ICipher cipher = new Default();
        switch (type)
        {
            case "b64f":
                cipher = new Base64Full();
                break;
            case "b648b":
                cipher = new Base648Bit();
                break;
            case "caesar":
                cipher = new Caesar();
                break;
            case "coltrans":
                cipher = new ColumnarTransposition();
                break;
            case "cycle":
                cipher = new CyclicPermutation();
                break;
            case "def":
                cipher = new Default();
                break;
            case "caesarl":
                cipher = new LameCaesar();
                break;
            case "rail":
                cipher = new RailFence();
                break;
            case "scytale":
                cipher = new Scytale();
                break;
            case "vigenere":
                cipher = new Vigenere();
                break;
        }
        return cipher;
    }

    public static string GetTooltip(string type)
    {
        string tip = Tooltip.Default;
        switch (type)
        {
            case "b64f":
                tip = Tooltip.B64F;
                break;
            case "b648b":
                tip = Tooltip.B648B;
                break;
            case "caesar":
                tip = Tooltip.Caesar;
                break;
            case "coltrans":
                tip = Tooltip.ColTrans;
                break;
            case "cycle":
                tip = Tooltip.Cycle;
                break;
            case "caesarl":
                tip = Tooltip.CaesarL;
                break;
            case "rail":
                tip = Tooltip.Rail;
                break;
            case "scytale":
                tip = Tooltip.Scytale;
                break;
            case "vigenere":
                tip = Tooltip.Vigenere;
                break;
        }
        return tip;
    }
}