using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RimGacha
{
	// Token: 0x0200000F RID: 15
	[HarmonyPatch(typeof(GenConstruct))]
	[HarmonyPatch("CanPlaceBlueprintOver")]
	public class Harmony_GenConstruct_CanPlaceBlueprintOver
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002A18 File Offset: 0x00000C18
		public static bool Prefix(ref bool __result, BuildableDef newDef, ThingDef oldDef)
		{
			bool flag = newDef == ThingDefOf.Gachapon && oldDef.thingClass == typeof(Building_GachaponCollection);
			bool result;
			if (flag)
			{
				__result = true;
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
