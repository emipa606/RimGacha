using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGacha
{
	// Token: 0x02000016 RID: 22
	public class Building_Art_Gachapon : Building_Art
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00003DF0 File Offset: 0x00001FF0
		public override Graphic Graphic
		{
			get
			{
				bool flag = this.graphicDataInt == null;
				if (flag)
				{
					PawnKindDef animal = this.TryGetComp<Comp_AnimalDesign>().Animal;
					PawnKindLifeStage pawnKindLifeStage = animal.lifeStages.Last<PawnKindLifeStage>();
					GraphicData graphicData = (pawnKindLifeStage.femaleGraphicData != null && Rand.ChanceSeeded(0.5f, this.GetHashCode())) ? pawnKindLifeStage.femaleGraphicData : pawnKindLifeStage.bodyGraphicData;
					float num = Building_Art_Gachapon.baseDrawSizeFromBodySize.Evaluate(animal.race.race.baseBodySize);
					bool flag2 = Building_Art_Gachapon.textureExceptionSize.ContainsKey(animal.defName);
					if (flag2)
					{
						num *= Building_Art_Gachapon.textureExceptionSize[animal.defName];
					}
					this.graphicDataInt = new GraphicData
					{
						texPath = graphicData.texPath,
						graphicClass = typeof(Graphic_Gachapon),
						shaderType = this.def.graphicData.shaderType,
						color = this.DrawColor,
						colorTwo = this.DrawColorTwo,
						drawSize = new Vector2(num, num),
						onGroundRandomRotateAngle = this.def.graphicData.onGroundRandomRotateAngle,
						drawRotated = graphicData.drawRotated,
						allowFlip = graphicData.allowFlip,
						flipExtraRotation = graphicData.flipExtraRotation,
						shadowData = this.def.graphicData.shadowData,
						damageData = this.def.graphicData.damageData,
						linkType = this.def.graphicData.linkType,
						linkFlags = this.def.graphicData.linkFlags
					};
				}
				return this.graphicDataInt.Graphic;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003FB4 File Offset: 0x000021B4
		public Graphic BlueprintGraphic
		{
			get
			{
				bool flag = this.cachedBlueprintGraphic == null;
				if (flag)
				{
					this.cachedBlueprintGraphic = ((Graphic_Gachapon)this.Graphic).GetColoredVersion(ShaderTypeDefOf.EdgeDetect, (Color)Building_Art_Gachapon.FI_BlueprintColor.GetValue(null), Color.white, null);
				}
				return this.cachedBlueprintGraphic;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000049 RID: 73 RVA: 0x0000400C File Offset: 0x0000220C
		public Graphic GhostGraphicValid
		{
			get
			{
				bool flag = this.cachedGhostGraphicValid == null;
				if (flag)
				{
					this.cachedGhostGraphicValid = ((Graphic_Gachapon)this.Graphic).GetColoredVersion(ShaderTypeDefOf.EdgeDetect, Designator_Place.CanPlaceColor, Color.white, null);
				}
				return this.cachedGhostGraphicValid;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600004A RID: 74 RVA: 0x0000405C File Offset: 0x0000225C
		public Graphic GhostGraphicInvalid
		{
			get
			{
				bool flag = this.cachedGhostGraphicInvalid == null;
				if (flag)
				{
					this.cachedGhostGraphicInvalid = ((Graphic_Gachapon)this.Graphic).GetColoredVersion(ShaderTypeDefOf.EdgeDetect, Designator_Place.CannotPlaceColor, Color.white, null);
				}
				return this.cachedGhostGraphicInvalid;
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000040AC File Offset: 0x000022AC
		public Graphic GetGhostGraphic(Color ghostCol)
		{
			bool flag = ghostCol == Designator_Place.CanPlaceColor;
			Graphic result;
			if (flag)
			{
				result = this.GhostGraphicValid;
			}
			else
			{
				result = this.GhostGraphicInvalid;
			}
			return result;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000040E0 File Offset: 0x000022E0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			Building_GachaponCollection building_GachaponCollection;
			bool flag = (building_GachaponCollection = (map.edificeGrid[base.Position] as Building_GachaponCollection)) != null;
			if (flag)
			{
				building_GachaponCollection.Register(this);
			}
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004120 File Offset: 0x00002320
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			IntVec3 position = base.Position;
			Building_GachaponCollection building_GachaponCollection = null;
			foreach (Thing thing in map.thingGrid.ThingsAt(position))
			{
				building_GachaponCollection = (thing as Building_GachaponCollection);
				bool flag = building_GachaponCollection != null;
				if (flag)
				{
					break;
				}
			}
			base.DeSpawn(mode);
			bool flag2 = building_GachaponCollection != null;
			if (flag2)
			{
				building_GachaponCollection.DeRegister(this);
				map.edificeGrid.InnerArray[map.cellIndices.CellToIndex(position)] = building_GachaponCollection;
			}
		}

		// Token: 0x04000029 RID: 41
		private static readonly FieldInfo FI_BlueprintColor = typeof(ThingDefGenerator_Buildings).GetField("BlueprintColor", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x0400002A RID: 42
		private static readonly SimpleCurve baseDrawSizeFromBodySize = new SimpleCurve
		{
			{
				0.2f,
				0.65f,
				true
			},
			{
				1f,
				1f,
				true
			},
			{
				2f,
				1.2f,
				true
			},
			{
				5f,
				1.4f,
				true
			}
		};

		// Token: 0x0400002B RID: 43
		private static readonly Dictionary<string, float> textureExceptionSize = new Dictionary<string, float>
		{
			{
				"Ostrich",
				0.95f
			},
			{
				"Elephant",
				0.75f
			},
			{
				"Muffalo",
				1.05f
			}
		};

		// Token: 0x0400002C RID: 44
		private GraphicData graphicDataInt;

		// Token: 0x0400002D RID: 45
		private Graphic cachedBlueprintGraphic;

		// Token: 0x0400002E RID: 46
		private Graphic cachedGhostGraphicValid;

		// Token: 0x0400002F RID: 47
		private Graphic cachedGhostGraphicInvalid;
	}
}
