using RimWorld;
using Verse;

namespace RimGacha
{
    // Token: 0x02000011 RID: 17
    public class StatPart_AnimalDesign : StatPart
    {
        // Token: 0x04000006 RID: 6
        private SimpleCurve projectionCurve;

        // Token: 0x0600001E RID: 30 RVA: 0x00002ABC File Offset: 0x00000CBC
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

        // Token: 0x0600001F RID: 31 RVA: 0x00002B08 File Offset: 0x00000D08
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

        // Token: 0x06000020 RID: 32 RVA: 0x00002B54 File Offset: 0x00000D54
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
}