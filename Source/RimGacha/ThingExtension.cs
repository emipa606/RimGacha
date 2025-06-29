using RimWorld;
using Verse;

namespace RimGacha;

public static class ThingExtension
{
    public static bool TryGetAnimalDesign(this Thing t, out PawnKindDef animal)
    {
        var compAnimalDesign = t is not MinifiedThing minifiedThing
            ? t.TryGetComp<Comp_AnimalDesign>()
            : minifiedThing.InnerThing.TryGetComp<Comp_AnimalDesign>();
        bool result;
        if (compAnimalDesign == null)
        {
            animal = null;
            result = false;
        }
        else
        {
            animal = compAnimalDesign.Animal;
            result = true;
        }

        return result;
    }
}