using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGacha;

[HarmonyPatch(typeof(Designator_Install), "DrawGhost")]
public static class Designator_Install_DrawGhost
{
    private static readonly PropertyInfo PI_ThingToInstall =
        typeof(Designator_Install).GetProperty("ThingToInstall", BindingFlags.Instance | BindingFlags.NonPublic);

    public static bool Prefix(Designator_Install __instance, Color ghostCol, Rot4 ___placingRot)
    {
        var thing = (Thing)PI_ThingToInstall.GetValue(__instance, null);
        if (thing.def != ThingDefOf.Gachapon)
        {
            return true;
        }

        if (thing is not Building_Art_Gachapon building_Art_Gachapon)
        {
            return true;
        }

        var ghostGraphic = building_Art_Gachapon.GetGhostGraphic(ghostCol);
        GachaponGhostDrawer.DrawGhostThing(UI.MouseCell(), ___placingRot, (ThingDef)__instance.PlacingDef, ghostGraphic,
            AltitudeLayer.PawnState);
        return false;
    }
}