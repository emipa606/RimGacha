using Verse;

namespace RimGacha;

public static class RacePropertiesExtension
{
    public static bool IsAnimal(this RaceProperties race)
    {
        return race.thinkTreeMain == ThinkTreeDefOf.Animal;
    }
}