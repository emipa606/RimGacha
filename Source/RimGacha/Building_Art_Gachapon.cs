using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGacha;

public class Building_Art_Gachapon : Building_Art
{
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

    private Graphic cachedBlueprintGraphic;

    private Graphic cachedGhostGraphicInvalid;

    private Graphic cachedGhostGraphicValid;

    private GraphicData graphicDataInt;

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
            if (textureExceptionSize.TryGetValue(animal.defName, out var value))
            {
                num *= value;
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

    public Graphic BlueprintGraphic
    {
        get
        {
            if (cachedBlueprintGraphic == null)
            {
                cachedBlueprintGraphic = ((Graphic_Gachapon)Graphic).GetColoredVersion(ShaderTypeDefOf.EdgeDetect,
                    ThingDefGenerator_Buildings.BlueprintColor, Color.white, null);
            }

            return cachedBlueprintGraphic;
        }
    }

    public Graphic GhostGraphicValid
    {
        get
        {
            if (cachedGhostGraphicValid == null)
            {
                cachedGhostGraphicValid = ((Graphic_Gachapon)Graphic).GetColoredVersion(ShaderTypeDefOf.EdgeDetect,
                    Designator_Place.CanPlaceColor, Color.white, null);
            }

            return cachedGhostGraphicValid;
        }
    }

    public Graphic GhostGraphicInvalid
    {
        get
        {
            if (cachedGhostGraphicInvalid == null)
            {
                cachedGhostGraphicInvalid =
                    ((Graphic_Gachapon)Graphic).GetColoredVersion(ShaderTypeDefOf.EdgeDetect,
                        Designator_Place.CannotPlaceColor, Color.white, null);
            }

            return cachedGhostGraphicInvalid;
        }
    }

    public Graphic GetGhostGraphic(Color ghostCol)
    {
        var result = ghostCol == Designator_Place.CanPlaceColor ? GhostGraphicValid : GhostGraphicInvalid;

        return result;
    }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        Building_GachaponCollection building_GachaponCollection;
        if ((building_GachaponCollection = map.edificeGrid[Position] as Building_GachaponCollection) != null)
        {
            building_GachaponCollection.Register(this);
        }

        base.SpawnSetup(map, respawningAfterLoad);
    }

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