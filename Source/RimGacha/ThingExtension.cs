using RimWorld;
using Verse;

namespace RimGacha
{
    // Token: 0x02000008 RID: 8
    public static class ThingExtension
    {
        // Token: 0x0600000B RID: 11 RVA: 0x0000262C File Offset: 0x0000082C
        public static bool TryGetAnimalDesign(this Thing t, out PawnKindDef animal)
        {
            var comp_AnimalDesign = !(t is MinifiedThing minifiedThing)
                ? t.TryGetComp<Comp_AnimalDesign>()
                : minifiedThing.InnerThing.TryGetComp<Comp_AnimalDesign>();
            bool result;
            if (comp_AnimalDesign == null)
            {
                animal = null;
                result = false;
            }
            else
            {
                animal = comp_AnimalDesign.Animal;
                result = true;
            }

            return result;
        }
    }
}