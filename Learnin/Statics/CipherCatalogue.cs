using Learnin.Ciphers;

namespace Learnin.Statics;

public class CipherCatalogue
{
    public static ICipher GetCipher(string type)
    {
        switch (type)
        {
            case "b64f":
                return new Base64Full();
            case "b648b":
                return new Base648Bit();
            case "caesar":
                return new Caesar();
            case "coltrans":
                return new ColumnarTransposition();
            case "cycle":
                return new CyclicPermutation();
            case "def":
                return new Default();
            case "caesarl":
                return new LameCaesar();
            case "rail":
                return new RailFence();
            case "scytale":
                return new Scytale();
            case "vigenere":
                return new Vigenere();
            default:
                return new Default();
        }
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