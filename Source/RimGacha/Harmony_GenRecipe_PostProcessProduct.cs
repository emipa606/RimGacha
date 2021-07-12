using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RimGacha
{
    // Token: 0x0200000E RID: 14
    [HarmonyPatch(typeof(GenRecipe))]
    [HarmonyPatch("PostProcessProduct")]
    public class Harmony_GenRecipe_PostProcessProduct
    {
        // Token: 0x06000018 RID: 24 RVA: 0x00002910 File Offset: 0x00000B10
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
}