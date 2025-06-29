using HarmonyLib;
using RimWorld;
using Verse;

namespace RimGacha;

[HarmonyPatch(typeof(GenConstruct), nameof(GenConstruct.BlocksConstruction))]
public static class GenConstruct_BlocksConstruction
{
    public static bool Prefix(ref bool __result, Thing constructible, Thing t)
    {
        if (t.def.thingClass != typeof(Building_GachaponCollection))
        {
            return true;
        }

        Blueprint_Install blueprintInstall;
        if ((blueprintInstall = constructible as Blueprint_Install) == null)
        {
            return true;
        }

        if (blueprintInstall.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified().def != ThingDefOf.Gachapon)
        {
            return true;
        }

        __result = false;
        return false;
    }
}