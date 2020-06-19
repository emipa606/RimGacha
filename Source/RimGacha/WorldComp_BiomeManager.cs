using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace RimGacha
{
	// Token: 0x02000018 RID: 24
	public class WorldComp_BiomeManager : WorldComponent
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000042B8 File Offset: 0x000024B8
		public int GlobalAnimalDefCount
		{
			get
			{
				return (this.cachedGlobalAnimalCommonalities == null) ? 0 : this.cachedGlobalAnimalCommonalities.Count;
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000042E0 File Offset: 0x000024E0
		public WorldComp_BiomeManager(World world) : base(world)
		{
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000042EC File Offset: 0x000024EC
		private void CreateGlobalAnimalCacheIfNeeded()
		{
			bool flag = this.cachedGlobalAnimalCommonalities == null;
			if (flag)
			{
				this.cachedGlobalAnimalCommonalities = new Dictionary<PawnKindDef, float>();
				foreach (Tile tile in this.world.grid.tiles)
				{
					BiomeDef biome = tile.biome;
					biome.CommonalityOfAnimal(PawnKindDefOf.Muffalo);
					Dictionary<PawnKindDef, float> dictionary = (Dictionary<PawnKindDef, float>)WorldComp_BiomeManager.FI_cachedAnimalCommonalities.GetValue(biome);
					foreach (KeyValuePair<PawnKindDef, float> keyValuePair in dictionary)
					{
						float num = keyValuePair.Value * biome.animalDensity;
						this.cachedGlobalAnimalCommonalities.EnsureKey(keyValuePair.Key, 0f);
						Dictionary<PawnKindDef, float> dictionary2 = this.cachedGlobalAnimalCommonalities;
						PawnKindDef key = keyValuePair.Key;
						dictionary2[key] += num;
						this.cachedGlobalAnimalCommonalitiesSum += num;
					}
				}
				foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
				{
					bool flag2 = pawnKindDef.race.race.IsAnimal() && !this.cachedGlobalAnimalCommonalities.ContainsKey(pawnKindDef);
					if (flag2)
					{
						float num2 = pawnKindDef.race.GetStatValueAbstract(StatDefOf.MarketValue, null);
						num2 = GenMath.LerpDouble(100f, 5000f, 0.005f, 0.0005f, num2) * this.cachedGlobalAnimalCommonalitiesSum;
						this.cachedGlobalAnimalCommonalities.Add(pawnKindDef, num2);
						this.cachedGlobalAnimalCommonalitiesSum += num2;
					}
				}
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000044F4 File Offset: 0x000026F4
		public PawnKindDef GetRandomGlobalAnimalWeighted()
		{
			this.CreateGlobalAnimalCacheIfNeeded();
			return this.cachedGlobalAnimalCommonalities.RandomElementByWeightWithFallback((KeyValuePair<PawnKindDef, float> x) => x.Value, WorldComp_BiomeManager.fallbackAnimalKVP).Key;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004544 File Offset: 0x00002744
		public float GetGlobalAnimalRarity(PawnKindDef animal)
		{
			this.CreateGlobalAnimalCacheIfNeeded();
			bool flag = !this.cachedGlobalAnimalCommonalities.ContainsKey(animal);
			float result;
			if (flag)
			{
				result = 0f;
			}
			else
			{
				result = this.cachedGlobalAnimalCommonalities[animal] / this.cachedGlobalAnimalCommonalitiesSum;
			}
			return result;
		}

		// Token: 0x04000030 RID: 48
		private static readonly FieldInfo FI_cachedAnimalCommonalities = typeof(BiomeDef).GetField("cachedAnimalCommonalities", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000031 RID: 49
		private static readonly KeyValuePair<PawnKindDef, float> fallbackAnimalKVP = new KeyValuePair<PawnKindDef, float>(PawnKindDefOf.Muffalo, 0f);

		// Token: 0x04000032 RID: 50
		[Unsaved]
		private Dictionary<PawnKindDef, float> cachedGlobalAnimalCommonalities;

		// Token: 0x04000033 RID: 51
		[Unsaved]
		private float cachedGlobalAnimalCommonalitiesSum;
	}
}
