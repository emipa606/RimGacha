using HarmonyLib;
using RimWorld;
using Verse;

namespace RimGacha;

[HarmonyPatch(typeof(GenConstruct))]
[HarmonyPatch("CanPlaceBlueprintOver")]
public class Harmony_GenConstruct_CanPlaceBlueprintOver
{
    public static bool Prefix(ref bool __result, BuildableDef newDef, ThingDef oldDef)
    {
        bool result;
        if (newDef == ThingDefOf.Gachapon && oldDef.thingClass == typeof(Building_GachaponCollection))
        {
            __result = true;
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }
}