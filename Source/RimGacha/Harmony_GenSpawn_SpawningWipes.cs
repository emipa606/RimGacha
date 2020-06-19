using System;
using HarmonyLib;
using Verse;

namespace RimGacha
{
	// Token: 0x0200000D RID: 13
	[HarmonyPatch(typeof(GenSpawn))]
	[HarmonyPatch("SpawningWipes")]
	public class Harmony_GenSpawn_SpawningWipes
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000028A0 File Offset: 0x00000AA0
		public static bool Prefix(ref bool __result, BuildableDef newEntDef, BuildableDef oldEntDef)
		{
			ThingDef thingDef = newEntDef as ThingDef;
			ThingDef thingDef2 = oldEntDef as ThingDef;
			bool flag = (newEntDef == ThingDefOf.Gachapon && thingDef2 != null && thingDef2.thingClass == typeof(Building_GachaponCollection)) || (oldEntDef == ThingDefOf.Gachapon && thingDef != null && thingDef.thingClass == typeof(Building_GachaponCollection));
			bool result;
			if (flag)
			{
				__result = false;
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
