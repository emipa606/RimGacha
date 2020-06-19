using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RimGacha
{
	// Token: 0x02000010 RID: 16
	[HarmonyPatch(typeof(GenConstruct))]
	[HarmonyPatch("BlocksConstruction")]
	public class Harmony_GenConstruct_BlocksConstruction
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002A54 File Offset: 0x00000C54
		public static bool Prefix(ref bool __result, Thing constructible, Thing t)
		{
			bool flag = t.def.thingClass == typeof(Building_GachaponCollection);
			if (flag)
			{
				Blueprint_Install blueprint_Install;
				bool flag2 = (blueprint_Install = (constructible as Blueprint_Install)) != null;
				if (flag2)
				{
					bool flag3 = blueprint_Install.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified().def == ThingDefOf.Gachapon;
					if (flag3)
					{
						__result = false;
						return false;
					}
				}
			}
			return true;
		}
	}
}
