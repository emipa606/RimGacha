using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RimGacha;

[HarmonyPatch(typeof(BeautyUtility))]
[HarmonyPatch("CellBeauty")]
public class Harmony_BeautyUtility_CellBeauty
{
    private static float BeautyOffsetHelper(Thing thing)
    {
        float result;
        if (thing is Building_GachaponCollection building_GachaponCollection)
        {
            result = building_GachaponCollection.BeautyOffset;
        }
        else
        {
            result = 0f;
        }

        return result;
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    {
        var i = 0;
        var iLen = instructions.Count();
        while (i < iLen)
        {
            var ci = instructions.ElementAt(i);
            if (ci.opcode == OpCodes.Stloc_S && instructions.ElementAt(i - 1).opcode == OpCodes.Call &&
                instructions.ElementAt(i - 2).opcode == OpCodes.Ldc_I4_1 &&
                instructions.ElementAt(i - 3).opcode == OpCodes.Ldsfld)
            {
                yield return ci;
                yield return instructions.ElementAt(i + 1);
                yield return new CodeInstruction(OpCodes.Call,
                    AccessTools.Method(typeof(Harmony_BeautyUtility_CellBeauty), "BeautyOffsetHelper"));
                yield return new CodeInstruction(OpCodes.Ldloc_S, ci.operand);
                yield return new CodeInstruction(OpCodes.Add);
            }

            yield return ci;
            var num = i;
            i = num + 1;
        }
    }
}