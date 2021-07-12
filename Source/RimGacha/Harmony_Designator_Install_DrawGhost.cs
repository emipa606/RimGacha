using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGacha
{
    // Token: 0x0200000A RID: 10
    [HarmonyPatch(typeof(Designator_Install))]
    [HarmonyPatch("DrawGhost")]
    public class Harmony_Designator_Install_DrawGhost
    {
        // Token: 0x04000004 RID: 4
        private static readonly FieldInfo FI_PlacingRot =
            typeof(Designator_Install).GetField("placingRot", BindingFlags.Instance | BindingFlags.NonPublic);

        // Token: 0x04000005 RID: 5
        private static readonly PropertyInfo PI_ThingToInstall =
            typeof(Designator_Install).GetProperty("ThingToInstall", BindingFlags.Instance | BindingFlags.NonPublic);

        // Token: 0x0600000F RID: 15 RVA: 0x000026C8 File Offset: 0x000008C8
        public static bool Prefix(Designator_Install __instance, Color ghostCol)
        {
            var thing = (Thing) PI_ThingToInstall.GetValue(__instance, null);
            if (thing.def != ThingDefOf.Gachapon)
            {
                return true;
            }

            if (!(thing is Building_Art_Gachapon building_Art_Gachapon))
            {
                return true;
            }

            var ghostGraphic = building_Art_Gachapon.GetGhostGraphic(ghostCol);
            GachaponGhostDrawer.DrawGhostThing(UI.MouseCell(), (Rot4) FI_PlacingRot.GetValue(__instance),
                (ThingDef) __instance.PlacingDef, ghostGraphic, AltitudeLayer.PawnState);
            return false;
        }
    }
}