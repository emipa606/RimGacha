using HarmonyLib;
using RimWorld;
using Verse;

namespace RimGacha;

[HarmonyPatch(typeof(Blueprint_Install))]
[HarmonyPatch("Graphic", MethodType.Getter)]
public class Harmony_Blueprint_Install_Graphic
{
    public static bool Prefix(Blueprint_Install __instance, ref Graphic __result)
    {
        if (__instance.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified().def != ThingDefOf.Gachapon)
        {
            return true;
        }

        if (!(__instance.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified() is Building_Art_Gachapon
                building_Art_Gachapon))
        {
            return true;
        }

        __result = building_Art_Gachapon.BlueprintGraphic;
        return false;
    }
}