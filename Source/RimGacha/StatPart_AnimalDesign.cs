using RimWorld;
using Verse;

namespace RimGacha;

public class StatPart_AnimalDesign : StatPart
{
    private SimpleCurve projectionCurve;

    public override void TransformValue(StatRequest req, ref float val)
    {
        if (!req.HasThing)
        {
            return;
        }

        var compAnimalDesign = req.Thing.TryGetComp<Comp_AnimalDesign>();
        if (compAnimalDesign != null)
        {
            val *= multiplier(req.Thing);
        }
    }

    public override string ExplanationPart(StatRequest req)
    {
        string result;
        if (!req.HasThing)
        {
            result = null;
        }
        else
        {
            result = "StatsReport_AnimalDesignMultiplier".Translate() + ": x" +
                     multiplier(req.Thing).ToStringPercent();
        }

        return result;
    }

    private float multiplier(Thing thing)
    {
        var num = 1f;
        var compAnimalDesign = thing.TryGetComp<Comp_AnimalDesign>();
        if (compAnimalDesign != null)
        {
            num *= projectionCurve.Evaluate(Find.World.GetComponent<WorldComp_BiomeManager>()
                .GetGlobalAnimalRarity(compAnimalDesign.Animal));
        }

        return num;
    }
}