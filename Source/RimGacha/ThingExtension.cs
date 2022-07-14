using RimWorld;
using Verse;

namespace RimGacha;

public static class ThingExtension
{
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