using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGacha;

public class Building_GachaponCollection : Building
{
    private const float GachaponBeautyFactorLowest = 1f;

    private const float GachaponBeautyFactorHighest = 0.8f;

    private const float CollectionBeautyFlatPerSetBonus = 5f;

    private const float CollectionBeautySetFactor = 0.08f;

    private const float SetPoints_Material = 1f;

    private const float SetPoints_Animal = 2f;

    private const float SetPoints_Predator = 2f;

    private const float SetPoints_Nuzzle = 2f;

    private const float SetPoints_SizeSmall = 1f;

    private const float SetPoints_SizeLarge = 1f;

    private const float SetPoints_BiomeRare = 2f;

    private const float SetPoints_BiomeCommon = 1f;

    private const float SetPoints_QualityLegendary = 2f;

    private const float SetPoints_QualityMasterwork = 1f;

    private static readonly FieldInfo FI_cachedAnimalCommonalities =
        typeof(BiomeDef).GetField("cachedAnimalCommonalities", BindingFlags.Instance | BindingFlags.NonPublic);

    private readonly List<Building_Art_Gachapon> collection = new List<Building_Art_Gachapon>();

    private float cachedBaseBeauty;

    private BiomeDef cachedSetDef_Biome;

    private bool cachedSetState_Animal;

    private bool cachedSetState_BiomeCommon;

    private bool cachedSetState_BiomeRare;

    private bool cachedSetState_Material;

    private bool cachedSetState_Nuzzle;

    private bool cachedSetState_Predator;

    private bool cachedSetState_QualityLegendary;

    private bool cachedSetState_QualityMasterwork;

    private bool cachedSetState_SizeLarge;

    private bool cachedSetState_SizeSmall;

    public float BeautyOffset
    {
        get
        {
            var collectionFullAndValid = CollectionFullAndValid;
            float result;
            if (collectionFullAndValid)
            {
                var num = 0f;
                if (cachedSetState_Material)
                {
                    num += 1f;
                }

                if (cachedSetState_Animal)
                {
                    num += 2f;
                }

                if (cachedSetState_Predator)
                {
                    num += 2f;
                }

                if (cachedSetState_Nuzzle)
                {
                    num += 2f;
                }

                if (cachedSetState_SizeSmall)
                {
                    num += 1f;
                }

                if (cachedSetState_SizeLarge)
                {
                    num += 1f;
                }

                if (cachedSetState_BiomeRare)
                {
                    num += 2f;
                }

                if (cachedSetState_BiomeCommon)
                {
                    num += 1f;
                }

                if (cachedSetState_QualityLegendary)
                {
                    num += 2f;
                }

                if (cachedSetState_QualityMasterwork)
                {
                    num += 1f;
                }

                result = Mathf.Round((5f + (0.08f * cachedBaseBeauty)) * num);
            }
            else
            {
                result = 0f;
            }

            return result;
        }
    }

    private bool CollectionFullAndValid
    {
        get
        {
            bool result;
            if (collection.Count == 4)
            {
                result = (from x in collection
                    group x by x).All(g => g.Count() == 1);
            }
            else
            {
                result = false;
            }

            return result;
        }
    }

    public override string GetInspectString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(base.GetInspectString());
        var spawned = Spawned;
        if (!spawned)
        {
            return stringBuilder.ToString().Trim();
        }

        if (stringBuilder.Length > 0)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(" ");
        }

        var collectionFullAndValid = CollectionFullAndValid;
        if (collectionFullAndValid)
        {
            var list = new List<string>();
            if (cachedSetState_Material)
            {
                list.Add("GachaponCollection_Set_Material".Translate());
            }

            if (cachedSetState_Animal)
            {
                list.Add("GachaponCollection_Set_Animal".Translate());
            }

            if (cachedSetState_Predator)
            {
                list.Add("GachaponCollection_Set_Predator".Translate());
            }

            if (cachedSetState_Nuzzle)
            {
                list.Add("GachaponCollection_Set_Nuzzle".Translate());
            }

            if (cachedSetState_SizeSmall)
            {
                list.Add("GachaponCollection_Set_SizeSmall".Translate());
            }

            if (cachedSetState_SizeLarge)
            {
                list.Add("GachaponCollection_Set_SizeLarge".Translate());
            }

            if (cachedSetState_BiomeRare)
            {
                list.Add("GachaponCollection_Set_BiomeRare".Translate(cachedSetDef_Biome.label));
            }

            if (cachedSetState_BiomeCommon)
            {
                list.Add("GachaponCollection_Set_BiomeCommon".Translate());
            }

            if (cachedSetState_QualityLegendary)
            {
                list.Add("GachaponCollection_Set_QualityLegendary".Translate());
            }

            if (cachedSetState_QualityMasterwork)
            {
                list.Add("GachaponCollection_Set_QualityMasterwork".Translate());
            }

            stringBuilder.AppendLine("GachaponCollection_CollectionList".Translate() + ": " +
                                     string.Join(", ", list.ToArray()));
            stringBuilder.AppendLine("GachaponCollection_BeautyBonus".Translate(BeautyOffset));
        }
        else
        {
            stringBuilder.AppendLine("GachaponCollection_Incomplete".Translate());
        }

        return stringBuilder.ToString().Trim();
    }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        var cellIndices = Map.cellIndices;
        var cellRect = this.OccupiedRect();
        for (var i = cellRect.minZ; i <= cellRect.maxZ; i++)
        {
            for (var j = cellRect.minX; j <= cellRect.maxX; j++)
            {
                var c = new IntVec3(j, 0, i);
                foreach (var thing in Map.thingGrid.ThingsAt(c))
                {
                    Building_Art_Gachapon building_Art_Gachapon;
                    if ((building_Art_Gachapon = thing as Building_Art_Gachapon) == null)
                    {
                        continue;
                    }

                    Register(building_Art_Gachapon);
                    Map.edificeGrid.InnerArray[cellIndices.CellToIndex(c)] = building_Art_Gachapon;
                    break;
                }
            }
        }
    }

    public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
    {
        base.DeSpawn(mode);
        collection.Clear();
        CalculateCollectionBaseBeauty();
        CalculateCollectionSets();
    }

    public void Register(Building_Art_Gachapon building)
    {
        collection.Add(building);
        CalculateCollectionBaseBeauty();
        CalculateCollectionSets();
    }

    public void DeRegister(Building_Art_Gachapon building)
    {
        collection.Remove(building);
        CalculateCollectionBaseBeauty();
        CalculateCollectionSets();
    }

    private void CalculateCollectionBaseBeauty()
    {
        cachedBaseBeauty = 0f;
        var collectionFullAndValid = CollectionFullAndValid;
        if (!collectionFullAndValid)
        {
            return;
        }

        var array = new float[collection.Count];
        var i = 0;
        var count = collection.Count;
        while (i < count)
        {
            array[i] = collection[i].GetStatValue(StatDefOf.Beauty);
            i++;
        }

        Array.Sort(array);
        var num = array[0] * 1f;
        var j = 1;
        var num2 = array.Length - 1;
        while (j < num2)
        {
            num += array[j];
            j++;
        }

        num += array[array.Length - 1] * 0.8f;
        cachedBaseBeauty = num;
    }

    private void CalculateCollectionSets()
    {
        cachedSetState_Material = false;
        cachedSetState_Animal = false;
        cachedSetState_Predator = false;
        cachedSetState_Nuzzle = false;
        cachedSetState_SizeSmall = false;
        cachedSetState_SizeLarge = false;
        cachedSetState_BiomeRare = false;
        cachedSetState_BiomeCommon = false;
        cachedSetState_QualityLegendary = false;
        cachedSetState_QualityMasterwork = false;
        cachedSetDef_Biome = null;
        var collectionFullAndValid = CollectionFullAndValid;
        if (!collectionFullAndValid)
        {
            return;
        }

        var firstStuff = collection[0].Stuff;
        cachedSetState_Material = collection.All(x => firstStuff == x.Stuff);
        var firstAnimal = collection[0].GetComp<Comp_AnimalDesign>().Animal;
        cachedSetState_Animal = collection.All(x => firstAnimal == x.GetComp<Comp_AnimalDesign>().Animal);
        if (!cachedSetState_Animal)
        {
            cachedSetState_Predator =
                collection.All(x => x.GetComp<Comp_AnimalDesign>().Animal.race.race.predator);
            cachedSetState_Nuzzle = collection.All(x =>
                x.GetComp<Comp_AnimalDesign>().Animal.race.race.nuzzleMtbHours > 0f);
            cachedSetState_SizeSmall = collection.All(x =>
                x.GetComp<Comp_AnimalDesign>().Animal.race.race.baseBodySize <= 0.5f);
            cachedSetState_SizeLarge = collection.All(x =>
                x.GetComp<Comp_AnimalDesign>().Animal.race.race.baseBodySize >= 2f);
            var list = new List<BiomeDef>();
            using (var enumerator = DefDatabase<BiomeDef>.AllDefs.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var biome = enumerator.Current;
                    if (collection.All(x =>
                            biome != null && biome.CommonalityOfAnimal(x.GetComp<Comp_AnimalDesign>().Animal) > 0f))
                    {
                        list.Add(biome);
                    }
                }
            }

            if (list.Count > 0)
            {
                list.Sort((b1, b2) =>
                    ((Dictionary<PawnKindDef, float>)FI_cachedAnimalCommonalities.GetValue(b1)).Count
                    .CompareTo(((Dictionary<PawnKindDef, float>)FI_cachedAnimalCommonalities.GetValue(b2))
                        .Count));
                cachedSetDef_Biome = list.Last();
                var num =
                    ((Dictionary<PawnKindDef, float>)FI_cachedAnimalCommonalities.GetValue(cachedSetDef_Biome))
                    .Count / (float)Find.World.GetComponent<WorldComp_BiomeManager>().GlobalAnimalDefCount;
                if (num < 0.1f)
                {
                    cachedSetState_BiomeRare = true;
                }
                else
                {
                    cachedSetState_BiomeCommon = true;
                }
            }
        }

        cachedSetState_QualityLegendary =
            collection.All(x => x.GetComp<CompQuality>().Quality == QualityCategory.Legendary);
        if (!cachedSetState_QualityLegendary)
        {
            cachedSetState_QualityMasterwork = collection.All(x =>
                x.GetComp<CompQuality>().Quality >= QualityCategory.Masterwork);
        }
    }
}