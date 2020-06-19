using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RimGacha
{
	// Token: 0x02000009 RID: 9
	[HarmonyPatch(typeof(BeautyUtility))]
	[HarmonyPatch("CellBeauty")]
	public class Harmony_BeautyUtility_CellBeauty
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002678 File Offset: 0x00000878
		private static float BeautyOffsetHelper(Thing thing)
		{
			Building_GachaponCollection building_GachaponCollection = thing as Building_GachaponCollection;
			bool flag = building_GachaponCollection != null;
			float result;
			if (flag)
			{
				result = building_GachaponCollection.BeautyOffset;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000026A8 File Offset: 0x000008A8
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
		{
			int i = 0;
			int iLen = instructions.Count<CodeInstruction>();
			while (i < iLen)
			{
				CodeInstruction ci = instructions.ElementAt(i);
				bool flag = ci.opcode == OpCodes.Stloc_S && instructions.ElementAt(i - 1).opcode == OpCodes.Call && instructions.ElementAt(i - 2).opcode == OpCodes.Ldc_I4_1 && instructions.ElementAt(i - 3).opcode == OpCodes.Ldsfld;
				if (flag)
				{
					yield return ci;
					yield return instructions.ElementAt(i + 1);
					yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Harmony_BeautyUtility_CellBeauty), "BeautyOffsetHelper", null, null));
					yield return new CodeInstruction(OpCodes.Ldloc_S, ci.operand);
					yield return new CodeInstruction(OpCodes.Add, null);
				}
				yield return ci;
				ci = null;
				int num = i;
				i = num + 1;
			}
			yield break;
		}
	}
}
