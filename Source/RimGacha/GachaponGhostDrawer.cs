using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGacha
{
	// Token: 0x02000012 RID: 18
	public static class GachaponGhostDrawer
	{
		// Token: 0x06000022 RID: 34 RVA: 0x00002BAC File Offset: 0x00000DAC
		public static void DrawGhostThing(IntVec3 center, Rot4 rot, ThingDef thingDef, Graphic ghostGraphic, AltitudeLayer drawAltitude)
		{
			Vector3 vector = GenThing.TrueCenter(center, rot, thingDef.Size, drawAltitude.AltitudeFor());
			ghostGraphic.DrawFromDef(vector, rot, thingDef, 0f);
			for (int i = 0; i < thingDef.comps.Count; i++)
			{
				thingDef.comps[i].DrawGhost(center, rot, thingDef, ghostGraphic.Color, drawAltitude);
			}
			bool flag = thingDef.PlaceWorkers != null;
			if (flag)
			{
				for (int j = 0; j < thingDef.PlaceWorkers.Count; j++)
				{
					thingDef.PlaceWorkers[j].DrawGhost(thingDef, center, rot, ghostGraphic.Color);
				}
			}
		}
	}
}
