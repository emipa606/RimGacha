using RimWorld;
using Verse;

namespace RimGacha
{
    // Token: 0x02000012 RID: 18
    public static class GachaponGhostDrawer
    {
        // Token: 0x06000022 RID: 34 RVA: 0x00002BAC File Offset: 0x00000DAC
        public static void DrawGhostThing(IntVec3 center, Rot4 rot, ThingDef thingDef, Graphic ghostGraphic,
            AltitudeLayer drawAltitude)
        {
            var vector = GenThing.TrueCenter(center, rot, thingDef.Size, drawAltitude.AltitudeFor());
            ghostGraphic.DrawFromDef(vector, rot, thingDef);
            foreach (var compProperties in thingDef.comps)
            {
                compProperties.DrawGhost(center, rot, thingDef, ghostGraphic.Color, drawAltitude);
            }

            if (thingDef.PlaceWorkers == null)
            {
                return;
            }

            foreach (var placeWorker in thingDef.PlaceWorkers)
            {
                placeWorker.DrawGhost(thingDef, center, rot, ghostGraphic.Color);
            }
        }
    }
}