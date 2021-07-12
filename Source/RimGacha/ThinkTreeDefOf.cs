using RimWorld;
using Verse;

namespace RimGacha
{
    // Token: 0x02000004 RID: 4
    [DefOf]
    public static class ThinkTreeDefOf
    {
        // Token: 0x04000003 RID: 3
        public static ThinkTreeDef Animal;

        // Token: 0x06000003 RID: 3 RVA: 0x00002076 File Offset: 0x00000276
        static ThinkTreeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThinkTreeDefOf));
        }
    }
}