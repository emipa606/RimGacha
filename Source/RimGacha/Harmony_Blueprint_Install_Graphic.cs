using System;
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
			bool flag = __instance.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified().def == ThingDefOf.Gachapon;
			if (flag)
			{
				Building_Art_Gachapon building_Art_Gachapon = __instance.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified() as Building_Art_Gachapon;
				bool flag2 = building_Art_Gachapon != null;
				if (flag2)
				{
					__result = building_Art_Gachapon.BlueprintGraphic;
					return false;
				}
			}
			return true;
		}
	}
}
