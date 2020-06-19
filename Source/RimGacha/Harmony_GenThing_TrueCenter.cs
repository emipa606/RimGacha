using System;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGacha
{
	// Token: 0x0200000C RID: 12
	[HarmonyPatch(typeof(GenThing))]
	[HarmonyPatch("TrueCenter")]
	[HarmonyPatch(new Type[]
	{
		typeof(Thing)
	})]
	public class Harmony_GenThing_TrueCenter
	{
		// Token: 0x06000014 RID: 20 RVA: 0x000027DC File Offset: 0x000009DC
		public static void Postfix(ref Vector3 __result, Thing t)
		{
			Blueprint_Install blueprint_Install;
			bool flag = t.def == ThingDefOf.Gachapon || ((blueprint_Install = (t as Blueprint_Install)) != null && blueprint_Install.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified().def == ThingDefOf.Gachapon);
			if (flag)
			{
				foreach (Thing thing in t.Map.thingGrid.ThingsAt(t.Position))
				{
					bool flag2 = thing.def.thingClass == typeof(Building_GachaponCollection);
					if (flag2)
					{
						__result.z += 0.1f;
						break;
					}
				}
			}
		}
	}
}
