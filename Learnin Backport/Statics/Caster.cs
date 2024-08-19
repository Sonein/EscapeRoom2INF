using Godot;
using Godot.Collections;

namespace Learnin.Statics;

public static class Caster
{
    public static Array<Polygon2D> CastToArrayPoly2D(object x)
    {
        Array xOut = x as Array;
        if (xOut != null)
        {
            Array<Polygon2D> output = new Array<Polygon2D>();
            foreach (var boi in xOut)
            {
                if (boi is Polygon2D boiNode)
                {
                    output.Add(boiNode);
                }
            }

            return output;
        }

        return new Array<Polygon2D>();
    }

    public static Array<string> CastToArrayString(object x)
    {
        Array xOut = x as Array;
        if (xOut != null)
        {
            Array<string> output = new Array<string>();
            foreach (var boi in xOut)
            {
                if (boi is string boiNode)
                {
                    output.Add(boiNode);
                }
            }

            return output;
        }
        
        return new Array<string>();
    }
}