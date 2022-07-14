using RimWorld;
using Verse;

namespace RimGacha;

[DefOf]
public static class ThinkTreeDefOf
{
    public static ThinkTreeDef Animal;

    static ThinkTreeDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(ThinkTreeDefOf));
    }
}