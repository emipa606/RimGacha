using System.Reflection;
using Verse;

namespace RimGacha;

[StaticConstructorOnStartup]
public class Harmony
{
    static Harmony()
    {
        var harmonyInstance = new HarmonyLib.Harmony("rimworld.lanilor.rimgacha");
        harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
    }
}