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
        // Token: 0x04000029 RID: 41
        private static readonly FieldInfo FI_BlueprintColor =
            typeof(ThingDefGenerator_Buildings).GetField("BlueprintColor",
                BindingFlags.Static | BindingFlags.NonPublic);

        // Token: 0x0400002A RID: 42
        private static readonly SimpleCurve baseDrawSizeFromBodySize = new SimpleCurve
        {
            {
                0.2f,
                0.65f
            },
            {
                1f,
                1f
            },
            {
                2f,
                1.2f
            },
            {
                5f,
                1.4f
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

        // Token: 0x0400002D RID: 45
        private Graphic cachedBlueprintGraphic;

        // Token: 0x0400002F RID: 47
        private Graphic cachedGhostGraphicInvalid;

        // Token: 0x0400002E RID: 46
        private Graphic cachedGhostGraphicValid;

        // Token: 0x0400002C RID: 44
        private GraphicData graphicDataInt;

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000047 RID: 71 RVA: 0x00003DF0 File Offset: 0x00001FF0
        public override Graphic Graphic
        {
            get
            {
                if (graphicDataInt != null)
                {
                    return graphicDataInt.Graphic;
                }

                var animal = this.TryGetComp<Comp_AnimalDesign>().Animal;
                var pawnKindLifeStage = animal.lifeStages.Last();
                var graphicData =
                    pawnKindLifeStage.femaleGraphicData != null && Rand.ChanceSeeded(0.5f, GetHashCode())
                        ? pawnKindLifeStage.femaleGraphicData
                        : pawnKindLifeStage.bodyGraphicData;
                var num = baseDrawSizeFromBodySize.Evaluate(animal.race.race.baseBodySize);
                if (textureExceptionSize.ContainsKey(animal.defName))
                {
                    num *= textureExceptionSize[animal.defName];
                }

                graphicDataInt = new GraphicData
                {
                    texPath = graphicData.texPath,
                    graphicClass = typeof(Graphic_Gachapon),
                    shaderType = def.graphicData.shaderType,
                    color = DrawColor,
                    colorTwo = DrawColorTwo,
                    drawSize = new Vector2(num, num),
                    onGroundRandomRotateAngle = def.graphicData.onGroundRandomRotateAngle,
                    drawRotated = graphicData.drawRotated,
                    allowFlip = graphicData.allowFlip,
                    flipExtraRotation = graphicData.flipExtraRotation,
                    shadowData = def.graphicData.shadowData,
                    damageData = def.graphicData.damageData,
                    linkType = def.graphicData.linkType,
                    linkFlags = def.graphicData.linkFlags
                };

                return graphicDataInt.Graphic;
            }
        }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000048 RID: 72 RVA: 0x00003FB4 File Offset: 0x000021B4
        public Graphic BlueprintGraphic
        {
            get
            {
                if (cachedBlueprintGraphic == null)
                {
                    cachedBlueprintGraphic = ((Graphic_Gachapon) Graphic).GetColoredVersion(ShaderTypeDefOf.EdgeDetect,
                        (Color) FI_BlueprintColor.GetValue(null), Color.white, null);
                }

                return cachedBlueprintGraphic;
            }
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000049 RID: 73 RVA: 0x0000400C File Offset: 0x0000220C
        public Graphic GhostGraphicValid
        {
            get
            {
                if (cachedGhostGraphicValid == null)
                {
                    cachedGhostGraphicValid = ((Graphic_Gachapon) Graphic).GetColoredVersion(ShaderTypeDefOf.EdgeDetect,
                        Designator_Place.CanPlaceColor, Color.white, null);
                }

                return cachedGhostGraphicValid;
            }
        }

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x0600004A RID: 74 RVA: 0x0000405C File Offset: 0x0000225C
        public Graphic GhostGraphicInvalid
        {
            get
            {
                if (cachedGhostGraphicInvalid == null)
                {
                    cachedGhostGraphicInvalid =
                        ((Graphic_Gachapon) Graphic).GetColoredVersion(ShaderTypeDefOf.EdgeDetect,
                            Designator_Place.CannotPlaceColor, Color.white, null);
                }

                return cachedGhostGraphicInvalid;
            }
        }

        // Token: 0x0600004B RID: 75 RVA: 0x000040AC File Offset: 0x000022AC
        public Graphic GetGhostGraphic(Color ghostCol)
        {
            var result = ghostCol == Designator_Place.CanPlaceColor ? GhostGraphicValid : GhostGraphicInvalid;

            return result;
        }

        // Token: 0x0600004C RID: 76 RVA: 0x000040E0 File Offset: 0x000022E0
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            Building_GachaponCollection building_GachaponCollection;
            if ((building_GachaponCollection = map.edificeGrid[Position] as Building_GachaponCollection) != null)
            {
                building_GachaponCollection.Register(this);
            }

            base.SpawnSetup(map, respawningAfterLoad);
        }

        // Token: 0x0600004D RID: 77 RVA: 0x00004120 File Offset: 0x00002320
        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            var map = Map;
            var position = Position;
            Building_GachaponCollection building_GachaponCollection = null;
            foreach (var thing in map.thingGrid.ThingsAt(position))
            {
                building_GachaponCollection = thing as Building_GachaponCollection;
                if (building_GachaponCollection != null)
                {
                    break;
                }
            }

            base.DeSpawn(mode);
            if (building_GachaponCollection == null)
            {
                return;
            }

            building_GachaponCollection.DeRegister(this);
            map.edificeGrid.InnerArray[map.cellIndices.CellToIndex(position)] = building_GachaponCollection;
        }
    }
}