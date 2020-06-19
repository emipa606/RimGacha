using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGacha
{
	// Token: 0x02000015 RID: 21
	public class Building_GachaponCollection : Building
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00003430 File Offset: 0x00001630
		public float BeautyOffset
		{
			get
			{
				bool collectionFullAndValid = this.CollectionFullAndValid;
				float result;
				if (collectionFullAndValid)
				{
					float num = 0f;
					bool flag = this.cachedSetState_Material;
					if (flag)
					{
						num += 1f;
					}
					bool flag2 = this.cachedSetState_Animal;
					if (flag2)
					{
						num += 2f;
					}
					bool flag3 = this.cachedSetState_Predator;
					if (flag3)
					{
						num += 2f;
					}
					bool flag4 = this.cachedSetState_Nuzzle;
					if (flag4)
					{
						num += 2f;
					}
					bool flag5 = this.cachedSetState_SizeSmall;
					if (flag5)
					{
						num += 1f;
					}
					bool flag6 = this.cachedSetState_SizeLarge;
					if (flag6)
					{
						num += 1f;
					}
					bool flag7 = this.cachedSetState_BiomeRare;
					if (flag7)
					{
						num += 2f;
					}
					bool flag8 = this.cachedSetState_BiomeCommon;
					if (flag8)
					{
						num += 1f;
					}
					bool flag9 = this.cachedSetState_QualityLegendary;
					if (flag9)
					{
						num += 2f;
					}
					bool flag10 = this.cachedSetState_QualityMasterwork;
					if (flag10)
					{
						num += 1f;
					}
					result = Mathf.Round((5f + 0.08f * this.cachedBaseBeauty) * num);
				}
				else
				{
					result = 0f;
				}
				return result;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003554 File Offset: 0x00001754
		private bool CollectionFullAndValid
		{
			get
			{
				bool result;
				if (this.collection.Count == 4)
				{
					result = (from x in this.collection
					group x by x).All((IGrouping<Building_Art_Gachapon, Building_Art_Gachapon> g) => g.Count<Building_Art_Gachapon>() == 1);
				}
				else
				{
					result = false;
				}
				return result;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000035C8 File Offset: 0x000017C8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			bool spawned = base.Spawned;
			if (spawned)
			{
				bool flag = stringBuilder.Length > 0;
				if (flag)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(" ");
				}
				bool collectionFullAndValid = this.CollectionFullAndValid;
				if (collectionFullAndValid)
				{
					List<string> list = new List<string>();
					bool flag2 = this.cachedSetState_Material;
					if (flag2)
					{
						list.Add(Translator.Translate("GachaponCollection_Set_Material"));
					}
					bool flag3 = this.cachedSetState_Animal;
					if (flag3)
					{
						list.Add(Translator.Translate("GachaponCollection_Set_Animal"));
					}
					bool flag4 = this.cachedSetState_Predator;
					if (flag4)
					{
						list.Add(Translator.Translate("GachaponCollection_Set_Predator"));
					}
					bool flag5 = this.cachedSetState_Nuzzle;
					if (flag5)
					{
						list.Add(Translator.Translate("GachaponCollection_Set_Nuzzle"));
					}
					bool flag6 = this.cachedSetState_SizeSmall;
					if (flag6)
					{
						list.Add(Translator.Translate("GachaponCollection_Set_SizeSmall"));
					}
					bool flag7 = this.cachedSetState_SizeLarge;
					if (flag7)
					{
						list.Add(Translator.Translate("GachaponCollection_Set_SizeLarge"));
					}
					bool flag8 = this.cachedSetState_BiomeRare;
					if (flag8)
					{
						list.Add(TranslatorFormattedStringExtensions.Translate("GachaponCollection_Set_BiomeRare", this.cachedSetDef_Biome.label));
					}
					bool flag9 = this.cachedSetState_BiomeCommon;
					if (flag9)
					{
						list.Add(Translator.Translate("GachaponCollection_Set_BiomeCommon"));
					}
					bool flag10 = this.cachedSetState_QualityLegendary;
					if (flag10)
					{
						list.Add(Translator.Translate("GachaponCollection_Set_QualityLegendary"));
					}
					bool flag11 = this.cachedSetState_QualityMasterwork;
					if (flag11)
					{
						list.Add(Translator.Translate("GachaponCollection_Set_QualityMasterwork"));
					}
					stringBuilder.AppendLine(Translator.Translate("GachaponCollection_CollectionList") + ": " + string.Join(", ", list.ToArray()));
					stringBuilder.AppendLine(TranslatorFormattedStringExtensions.Translate("GachaponCollection_BeautyBonus", this.BeautyOffset));
				}
				else
				{
					stringBuilder.AppendLine(Translator.Translate("GachaponCollection_Incomplete"));
				}
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000037F0 File Offset: 0x000019F0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			CellIndices cellIndices = base.Map.cellIndices;
			CellRect cellRect = this.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					foreach (Thing thing in base.Map.thingGrid.ThingsAt(c))
					{
						Building_Art_Gachapon building_Art_Gachapon;
						bool flag = (building_Art_Gachapon = (thing as Building_Art_Gachapon)) != null;
						if (flag)
						{
							this.Register(building_Art_Gachapon);
							base.Map.edificeGrid.InnerArray[cellIndices.CellToIndex(c)] = building_Art_Gachapon;
							break;
						}
					}
				}
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000038FC File Offset: 0x00001AFC
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.collection.Clear();
			this.CalculateCollectionBaseBeauty();
			this.CalculateCollectionSets();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003921 File Offset: 0x00001B21
		public void Register(Building_Art_Gachapon building)
		{
			this.collection.Add(building);
			this.CalculateCollectionBaseBeauty();
			this.CalculateCollectionSets();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000393F File Offset: 0x00001B3F
		public void DeRegister(Building_Art_Gachapon building)
		{
			this.collection.Remove(building);
			this.CalculateCollectionBaseBeauty();
			this.CalculateCollectionSets();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003960 File Offset: 0x00001B60
		private void CalculateCollectionBaseBeauty()
		{
			this.cachedBaseBeauty = 0f;
			bool collectionFullAndValid = this.CollectionFullAndValid;
			if (collectionFullAndValid)
			{
				float[] array = new float[this.collection.Count];
				int i = 0;
				int count = this.collection.Count;
				while (i < count)
				{
					array[i] = this.collection[i].GetStatValue(StatDefOf.Beauty, true);
					i++;
				}
				Array.Sort<float>(array);
				float num = array[0] * 1f;
				int j = 1;
				int num2 = array.Length - 1;
				while (j < num2)
				{
					num += array[j];
					j++;
				}
				num += array[array.Length - 1] * 0.8f;
				this.cachedBaseBeauty = num;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003A24 File Offset: 0x00001C24
		private void CalculateCollectionSets()
		{
			this.cachedSetState_Material = false;
			this.cachedSetState_Animal = false;
			this.cachedSetState_Predator = false;
			this.cachedSetState_Nuzzle = false;
			this.cachedSetState_SizeSmall = false;
			this.cachedSetState_SizeLarge = false;
			this.cachedSetState_BiomeRare = false;
			this.cachedSetState_BiomeCommon = false;
			this.cachedSetState_QualityLegendary = false;
			this.cachedSetState_QualityMasterwork = false;
			this.cachedSetDef_Biome = null;
			bool collectionFullAndValid = this.CollectionFullAndValid;
			if (collectionFullAndValid)
			{
				ThingDef firstStuff = this.collection[0].Stuff;
				this.cachedSetState_Material = this.collection.All((Building_Art_Gachapon x) => firstStuff == x.Stuff);
				PawnKindDef firstAnimal = this.collection[0].GetComp<Comp_AnimalDesign>().Animal;
				this.cachedSetState_Animal = this.collection.All((Building_Art_Gachapon x) => firstAnimal == x.GetComp<Comp_AnimalDesign>().Animal);
				bool flag = !this.cachedSetState_Animal;
				if (flag)
				{
					this.cachedSetState_Predator = this.collection.All((Building_Art_Gachapon x) => x.GetComp<Comp_AnimalDesign>().Animal.race.race.predator);
					this.cachedSetState_Nuzzle = this.collection.All((Building_Art_Gachapon x) => x.GetComp<Comp_AnimalDesign>().Animal.race.race.nuzzleMtbHours > 0f);
					this.cachedSetState_SizeSmall = this.collection.All((Building_Art_Gachapon x) => x.GetComp<Comp_AnimalDesign>().Animal.race.race.baseBodySize <= 0.5f);
					this.cachedSetState_SizeLarge = this.collection.All((Building_Art_Gachapon x) => x.GetComp<Comp_AnimalDesign>().Animal.race.race.baseBodySize >= 2f);
					List<BiomeDef> list = new List<BiomeDef>();
					using (IEnumerator<BiomeDef> enumerator = DefDatabase<BiomeDef>.AllDefs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BiomeDef biome = enumerator.Current;
							bool flag2 = this.collection.All((Building_Art_Gachapon x) => biome.CommonalityOfAnimal(x.GetComp<Comp_AnimalDesign>().Animal) > 0f);
							if (flag2)
							{
								list.Add(biome);
							}
						}
					}
					bool flag3 = list.Count > 0;
					if (flag3)
					{
						list.Sort((BiomeDef b1, BiomeDef b2) => ((Dictionary<PawnKindDef, float>)Building_GachaponCollection.FI_cachedAnimalCommonalities.GetValue(b1)).Count.CompareTo(((Dictionary<PawnKindDef, float>)Building_GachaponCollection.FI_cachedAnimalCommonalities.GetValue(b2)).Count));
						this.cachedSetDef_Biome = list.Last<BiomeDef>();
						float num = (float)(((Dictionary<PawnKindDef, float>)Building_GachaponCollection.FI_cachedAnimalCommonalities.GetValue(this.cachedSetDef_Biome)).Count / Find.World.GetComponent<WorldComp_BiomeManager>().GlobalAnimalDefCount);
						bool flag4 = num < 0.1f;
						if (flag4)
						{
							this.cachedSetState_BiomeRare = true;
						}
						else
						{
							this.cachedSetState_BiomeCommon = true;
						}
					}
				}
				this.cachedSetState_QualityLegendary = this.collection.All((Building_Art_Gachapon x) => x.GetComp<CompQuality>().Quality == QualityCategory.Legendary);
				bool flag5 = !this.cachedSetState_QualityLegendary;
				if (flag5)
				{
					this.cachedSetState_QualityMasterwork = this.collection.All((Building_Art_Gachapon x) => x.GetComp<CompQuality>().Quality >= QualityCategory.Masterwork);
				}
			}
		}

		// Token: 0x0400000D RID: 13
		private const float GachaponBeautyFactorLowest = 1f;

		// Token: 0x0400000E RID: 14
		private const float GachaponBeautyFactorHighest = 0.8f;

		// Token: 0x0400000F RID: 15
		private const float CollectionBeautyFlatPerSetBonus = 5f;

		// Token: 0x04000010 RID: 16
		private const float CollectionBeautySetFactor = 0.08f;

		// Token: 0x04000011 RID: 17
		private const float SetPoints_Material = 1f;

		// Token: 0x04000012 RID: 18
		private const float SetPoints_Animal = 2f;

		// Token: 0x04000013 RID: 19
		private const float SetPoints_Predator = 2f;

		// Token: 0x04000014 RID: 20
		private const float SetPoints_Nuzzle = 2f;

		// Token: 0x04000015 RID: 21
		private const float SetPoints_SizeSmall = 1f;

		// Token: 0x04000016 RID: 22
		private const float SetPoints_SizeLarge = 1f;

		// Token: 0x04000017 RID: 23
		private const float SetPoints_BiomeRare = 2f;

		// Token: 0x04000018 RID: 24
		private const float SetPoints_BiomeCommon = 1f;

		// Token: 0x04000019 RID: 25
		private const float SetPoints_QualityLegendary = 2f;

		// Token: 0x0400001A RID: 26
		private const float SetPoints_QualityMasterwork = 1f;

		// Token: 0x0400001B RID: 27
		private static readonly FieldInfo FI_cachedAnimalCommonalities = typeof(BiomeDef).GetField("cachedAnimalCommonalities", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x0400001C RID: 28
		private List<Building_Art_Gachapon> collection = new List<Building_Art_Gachapon>();

		// Token: 0x0400001D RID: 29
		private float cachedBaseBeauty = 0f;

		// Token: 0x0400001E RID: 30
		private bool cachedSetState_Material = false;

		// Token: 0x0400001F RID: 31
		private bool cachedSetState_Animal = false;

		// Token: 0x04000020 RID: 32
		private bool cachedSetState_Predator = false;

		// Token: 0x04000021 RID: 33
		private bool cachedSetState_Nuzzle = false;

		// Token: 0x04000022 RID: 34
		private bool cachedSetState_SizeSmall = false;

		// Token: 0x04000023 RID: 35
		private bool cachedSetState_SizeLarge = false;

		// Token: 0x04000024 RID: 36
		private bool cachedSetState_BiomeRare = false;

		// Token: 0x04000025 RID: 37
		private bool cachedSetState_BiomeCommon = false;

		// Token: 0x04000026 RID: 38
		private bool cachedSetState_QualityLegendary = false;

		// Token: 0x04000027 RID: 39
		private bool cachedSetState_QualityMasterwork = false;

		// Token: 0x04000028 RID: 40
		private BiomeDef cachedSetDef_Biome = null;
	}
}
