using System;
using RimWorld;
using Verse;

namespace RimGacha
{
	// Token: 0x02000011 RID: 17
	public class StatPart_AnimalDesign : StatPart
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00002ABC File Offset: 0x00000CBC
		public override void TransformValue(StatRequest req, ref float val)
		{
			bool flag = !req.HasThing;
			if (!flag)
			{
				Comp_AnimalDesign comp_AnimalDesign = req.Thing.TryGetComp<Comp_AnimalDesign>();
				bool flag2 = comp_AnimalDesign == null;
				if (!flag2)
				{
					val *= this.Multiplier(req.Thing);
				}
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002B08 File Offset: 0x00000D08
		public override string ExplanationPart(StatRequest req)
		{
			bool flag = !req.HasThing;
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = Translator.Translate("StatsReport_AnimalDesignMultiplier") + ": x" + this.Multiplier(req.Thing).ToStringPercent();
			}
			return result;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002B54 File Offset: 0x00000D54
		private float Multiplier(Thing thing)
		{
			float num = 1f;
			Comp_AnimalDesign comp_AnimalDesign = thing.TryGetComp<Comp_AnimalDesign>();
			bool flag = comp_AnimalDesign != null;
			if (flag)
			{
				num *= this.projectionCurve.Evaluate(Find.World.GetComponent<WorldComp_BiomeManager>().GetGlobalAnimalRarity(comp_AnimalDesign.Animal));
			}
			return num;
		}

		// Token: 0x04000006 RID: 6
		private SimpleCurve projectionCurve;
	}
}
