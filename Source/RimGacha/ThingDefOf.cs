using RimWorld;
using Verse;

namespace RimGacha;

[DefOf]
public static class ThingDefOf
{
    public static ThingDef Gachapon;

    static ThingDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
    }
}