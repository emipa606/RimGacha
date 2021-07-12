using RimWorld;
using Verse;

namespace RimGacha
{
    // Token: 0x02000003 RID: 3
    [DefOf]
    public static class ThingDefOf
    {
        // Token: 0x04000002 RID: 2
        public static ThingDef Gachapon;

        // Token: 0x06000002 RID: 2 RVA: 0x00002063 File Offset: 0x00000263
        static ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
        }
    }
}