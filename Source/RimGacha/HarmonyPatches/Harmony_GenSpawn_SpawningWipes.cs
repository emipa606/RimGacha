using HarmonyLib;
using Verse;

namespace RimGacha;

[HarmonyPatch(typeof(GenSpawn), nameof(GenSpawn.SpawningWipes))]
public static class Harmony_GenSpawn_SpawningWipes
{
    public static bool Prefix(ref bool __result, BuildableDef newEntDef, BuildableDef oldEntDef)
    {
        bool result;
        if (newEntDef == ThingDefOf.Gachapon && oldEntDef is ThingDef thingDef2 &&
            thingDef2.thingClass == typeof(Building_GachaponCollection) || oldEntDef == ThingDefOf.Gachapon &&
            newEntDef is ThingDef thingDef && thingDef.thingClass == typeof(Building_GachaponCollection))
        {
            __result = false;
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }
}