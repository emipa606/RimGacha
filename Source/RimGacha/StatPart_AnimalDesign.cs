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

        var comp_AnimalDesign = req.Thing.TryGetComp<Comp_AnimalDesign>();
        if (comp_AnimalDesign != null)
        {
            val *= Multiplier(req.Thing);
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
                     Multiplier(req.Thing).ToStringPercent();
        }

        return result;
    }

    private float Multiplier(Thing thing)
    {
        var num = 1f;
        var comp_AnimalDesign = thing.TryGetComp<Comp_AnimalDesign>();
        if (comp_AnimalDesign != null)
        {
            num *= projectionCurve.Evaluate(Find.World.GetComponent<WorldComp_BiomeManager>()
                .GetGlobalAnimalRarity(comp_AnimalDesign.Animal));
        }

        return num;
    }
}