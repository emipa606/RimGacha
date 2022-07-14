using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace RimGacha;

public class WorldComp_BiomeManager : WorldComponent
{
    private static readonly FieldInfo FI_cachedAnimalCommonalities =
        typeof(BiomeDef).GetField("cachedAnimalCommonalities", BindingFlags.Instance | BindingFlags.NonPublic);

    private static readonly KeyValuePair<PawnKindDef, float> fallbackAnimalKVP =
        new KeyValuePair<PawnKindDef, float>(PawnKindDefOf.Muffalo, 0f);

    [Unsaved] private Dictionary<PawnKindDef, float> cachedGlobalAnimalCommonalities;

    [Unsaved] private float cachedGlobalAnimalCommonalitiesSum;

    public WorldComp_BiomeManager(World world) : base(world)
    {
    }

    public int GlobalAnimalDefCount =>
        cachedGlobalAnimalCommonalities?.Count ?? 0;

    private void CreateGlobalAnimalCacheIfNeeded()
    {
        if (cachedGlobalAnimalCommonalities != null)
        {
            return;
        }

        cachedGlobalAnimalCommonalities = new Dictionary<PawnKindDef, float>();
        foreach (var tile in world.grid.tiles)
        {
            var biome = tile.biome;
            biome.CommonalityOfAnimal(PawnKindDefOf.Muffalo);
            var dictionary = (Dictionary<PawnKindDef, float>)FI_cachedAnimalCommonalities.GetValue(biome);
            foreach (var keyValuePair in dictionary)
            {
                var num = keyValuePair.Value * biome.animalDensity;
                cachedGlobalAnimalCommonalities.EnsureKey(keyValuePair.Key, 0f);
                var dictionary2 = cachedGlobalAnimalCommonalities;
                var key = keyValuePair.Key;
                dictionary2[key] += num;
                cachedGlobalAnimalCommonalitiesSum += num;
            }
        }

        foreach (var pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
        {
            if (!pawnKindDef.race.race.IsAnimal() || cachedGlobalAnimalCommonalities.ContainsKey(pawnKindDef))
            {
                continue;
            }

            var num2 = pawnKindDef.race.GetStatValueAbstract(StatDefOf.MarketValue);
            num2 = GenMath.LerpDouble(100f, 5000f, 0.005f, 0.0005f, num2) *
                   cachedGlobalAnimalCommonalitiesSum;
            cachedGlobalAnimalCommonalities.Add(pawnKindDef, num2);
            cachedGlobalAnimalCommonalitiesSum += num2;
        }
    }

    public PawnKindDef GetRandomGlobalAnimalWeighted()
    {
        CreateGlobalAnimalCacheIfNeeded();
        return cachedGlobalAnimalCommonalities.RandomElementByWeightWithFallback(x => x.Value, fallbackAnimalKVP)
            .Key;
    }

    public float GetGlobalAnimalRarity(PawnKindDef animal)
    {
        CreateGlobalAnimalCacheIfNeeded();
        float result;
        if (!cachedGlobalAnimalCommonalities.ContainsKey(animal))
        {
            result = 0f;
        }
        else
        {
            result = cachedGlobalAnimalCommonalities[animal] / cachedGlobalAnimalCommonalitiesSum;
        }

        return result;
    }
}