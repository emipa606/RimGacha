using System.Reflection;
using Verse;

namespace RimGacha
{
    // Token: 0x02000017 RID: 23
    [StaticConstructorOnStartup]
    public class Harmony
    {
        // Token: 0x06000050 RID: 80 RVA: 0x00004290 File Offset: 0x00002490
        static Harmony()
        {
            var harmonyInstance = new HarmonyLib.Harmony("rimworld.lanilor.rimgacha");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}