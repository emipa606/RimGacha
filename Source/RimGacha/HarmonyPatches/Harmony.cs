using System.Reflection;
using Verse;

namespace RimGacha.HarmonyPatches;

[StaticConstructorOnStartup]
public static class Harmony
{
    static Harmony()
    {
        new HarmonyLib.Harmony("rimworld.lanilor.rimgacha").PatchAll(Assembly.GetExecutingAssembly());
    }
}