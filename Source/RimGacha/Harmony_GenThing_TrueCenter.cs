using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGacha;

[HarmonyPatch(typeof(GenThing))]
[HarmonyPatch("TrueCenter")]
[HarmonyPatch(new[]
{
    typeof(Thing)
})]
public class Harmony_GenThing_TrueCenter
{
    public static void Postfix(ref Vector3 __result, Thing t)
    {
        Blueprint_Install blueprint_Install;
        if (t.def != ThingDefOf.Gachapon && ((blueprint_Install = t as Blueprint_Install) == null ||
                                             blueprint_Install.MiniToInstallOrBuildingToReinstall
                                                 .GetInnerIfMinified().def != ThingDefOf.Gachapon))
        {
            return;
        }

        foreach (var thing in t.Map.thingGrid.ThingsAt(t.Position))
        {
            if (thing.def.thingClass != typeof(Building_GachaponCollection))
            {
                continue;
            }

            __result.z += 0.1f;
            break;
        }
    }
}