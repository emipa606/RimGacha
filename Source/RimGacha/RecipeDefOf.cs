using RimWorld;
using Verse;

namespace RimGacha;

[DefOf]
public static class RecipeDefOf
{
    public static RecipeDef Make_Gachapon;

    static RecipeDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(RecipeDefOf));
    }
}