using HarmonyLib;
using RimWorld;
using Verse;

namespace RimGacha;

[HarmonyPatch(typeof(Blueprint), nameof(Blueprint.Graphic), MethodType.Getter)]
public static class Harmony_Blueprint_Install_Graphic
{
    public static void Postfix(Blueprint __instance, ref Graphic __result)
    {
        if (__instance is not Blueprint_Install blueprintInstall)
        {
            return;
        }

        var innerThing = blueprintInstall.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();

        if (innerThing == null || innerThing.def != ThingDefOf.Gachapon)
        {
            return;
        }

        if (innerThing is not Building_Art_Gachapon building_Art_Gachapon)
        {
            return;
        }

        var foundGraphic = building_Art_Gachapon.BlueprintGraphic;
        __result = foundGraphic;
    }
}