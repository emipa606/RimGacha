using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RimGacha;

[HarmonyPatch(typeof(BeautyUtility), nameof(BeautyUtility.CellBeauty))]
public static class BeautyUtility_CellBeauty
{
    private static float beautyOffsetHelper(Thing thing)
    {
        float result;
        if (thing is Building_GachaponCollection buildingGachaponCollection)
        {
            result = buildingGachaponCollection.BeautyOffset;
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
        var codeInstructions = instructions as CodeInstruction[] ?? instructions.ToArray();
        var iLen = codeInstructions.Length;
        while (i < iLen)
        {
            var ci = codeInstructions[i];
            if (ci.opcode == OpCodes.Stloc_S && codeInstructions[i - 1].opcode == OpCodes.Call &&
                codeInstructions[i - 2].opcode == OpCodes.Ldc_I4_1 &&
                codeInstructions[i - 3].opcode == OpCodes.Ldsfld)
            {
                yield return ci;
                yield return codeInstructions[i + 1];
                yield return new CodeInstruction(OpCodes.Call,
                    AccessTools.Method(typeof(BeautyUtility_CellBeauty), nameof(beautyOffsetHelper)));
                yield return new CodeInstruction(OpCodes.Ldloc_S, ci.operand);
                yield return new CodeInstruction(OpCodes.Add);
            }

            yield return ci;
            var num = i;
            i = num + 1;
        }
    }
}