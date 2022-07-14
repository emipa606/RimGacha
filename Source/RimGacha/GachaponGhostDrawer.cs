using RimWorld;
using Verse;

namespace RimGacha;

public static class GachaponGhostDrawer
{
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