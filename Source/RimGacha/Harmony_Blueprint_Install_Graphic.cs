using HarmonyLib;
using RimWorld;
using Verse;

namespace RimGacha
{
    // Token: 0x0200000B RID: 11
    [HarmonyPatch(typeof(Blueprint_Install))]
    [HarmonyPatch("Graphic", MethodType.Getter)]
    public class Harmony_Blueprint_Install_Graphic
    {
        // Token: 0x06000012 RID: 18 RVA: 0x00002784 File Offset: 0x00000984
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
}