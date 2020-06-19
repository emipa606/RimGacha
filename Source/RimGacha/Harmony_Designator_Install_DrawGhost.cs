using System;
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
		// Token: 0x0600000F RID: 15 RVA: 0x000026C8 File Offset: 0x000008C8
		public static bool Prefix(Designator_Install __instance, Color ghostCol)
		{
			Thing thing = (Thing)Harmony_Designator_Install_DrawGhost.PI_ThingToInstall.GetValue(__instance, null);
			bool flag = thing.def == ThingDefOf.Gachapon;
			if (flag)
			{
				Building_Art_Gachapon building_Art_Gachapon = thing as Building_Art_Gachapon;
				bool flag2 = building_Art_Gachapon != null;
				if (flag2)
				{
					Graphic ghostGraphic = building_Art_Gachapon.GetGhostGraphic(ghostCol);
					GachaponGhostDrawer.DrawGhostThing(UI.MouseCell(), (Rot4)Harmony_Designator_Install_DrawGhost.FI_PlacingRot.GetValue(__instance), (ThingDef)__instance.PlacingDef, ghostGraphic, AltitudeLayer.PawnState);
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000004 RID: 4
		private static readonly FieldInfo FI_PlacingRot = typeof(Designator_Install).GetField("placingRot", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000005 RID: 5
		private static readonly PropertyInfo PI_ThingToInstall = typeof(Designator_Install).GetProperty("ThingToInstall", BindingFlags.Instance | BindingFlags.NonPublic);
	}
}
