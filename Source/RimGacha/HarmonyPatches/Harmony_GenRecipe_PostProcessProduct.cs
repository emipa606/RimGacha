using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RimGacha;

[HarmonyPatch(typeof(GenRecipe), "PostProcessProduct")]
public static class Harmony_GenRecipe_PostProcessProduct
{
    public static bool Prefix(ref Thing __result, Thing product, RecipeDef recipeDef)
    {
        bool result;
        if (recipeDef == RecipeDefOf.Make_Gachapon)
        {
            var product1 = product;
            var source = from def in DefDatabase<ThingDef>.AllDefs
                where def.IsStuff && def.stuffProps.CanMake(product1.def)
                select def;
            var stuffDirect =
                source.RandomElementByWeightWithFallback(x => x.stuffProps.commonality, RimWorld.ThingDefOf.Steel);
            product.SetStuffDirect(stuffDirect);
            var compQuality = product.TryGetComp<CompQuality>();
            if (compQuality != null)
            {
                var q = QualityUtility.GenerateQualityCreatedByPawn(6, Rand.Chance(0.2f));
                compQuality.SetQuality(q, ArtGenerationContext.Colony);
            }

            product.HitPoints = product.MaxHitPoints;
            var minifiable = product.def.Minifiable;
            if (minifiable)
            {
                product = product.MakeMinified();
            }

            __result = product;
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }
}