using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGacha;

[HarmonyPatch(typeof(GenThing), nameof(GenThing.TrueCenter), typeof(Thing))]
public static class Harmony_GenThing_TrueCenter
{
    public static void Postfix(ref Vector3 __result, Thing t)
    {
        if (t == null)
        {
            return;
        }

        if (t.def != ThingDefOf.Gachapon)
        {
            if (t is not Blueprint_Install blueprint_Install)
            {
                return;
            }

            if (blueprint_Install.MiniToInstallOrBuildingToReinstall?.GetInnerIfMinified()
                    ?.def != ThingDefOf.Gachapon)
            {
                return;
            }
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