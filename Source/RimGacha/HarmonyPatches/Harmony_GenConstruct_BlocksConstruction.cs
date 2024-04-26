using HarmonyLib;
using RimWorld;
using Verse;

namespace RimGacha;

[HarmonyPatch(typeof(GenConstruct), nameof(GenConstruct.BlocksConstruction))]
public static class Harmony_GenConstruct_BlocksConstruction
{
    public static bool Prefix(ref bool __result, Thing constructible, Thing t)
    {
        if (t.def.thingClass != typeof(Building_GachaponCollection))
        {
            return true;
        }

        Blueprint_Install blueprint_Install;
        if ((blueprint_Install = constructible as Blueprint_Install) == null)
        {
            return true;
        }

        if (blueprint_Install.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified().def != ThingDefOf.Gachapon)
        {
            return true;
        }

        __result = false;
        return false;
    }
}