using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGacha;

public class Building_Art_Gachapon : Building_Art
{
    private static readonly SimpleCurve baseDrawSizeFromBodySize = new()
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

    private static readonly Dictionary<string, float> textureExceptionSize = new()
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
            field ??= ((Graphic_Gachapon)Graphic).GetColoredVersion(ShaderTypeDefOf.EdgeDetect,
                ThingDefGenerator_Buildings.BlueprintColor, Color.white, null);

            return field;
        }
    }

    private Graphic GhostGraphicValid
    {
        get
        {
            field ??= ((Graphic_Gachapon)Graphic).GetColoredVersion(ShaderTypeDefOf.EdgeDetect,
                Designator_Place.CanPlaceColor, Color.white, null);

            return field;
        }
    }

    private Graphic GhostGraphicInvalid
    {
        get
        {
            field ??= ((Graphic_Gachapon)Graphic).GetColoredVersion(ShaderTypeDefOf.EdgeDetect,
                Designator_Place.CannotPlaceColor, Color.white, null);

            return field;
        }
    }

    public Graphic GetGhostGraphic(Color ghostCol)
    {
        var result = ghostCol == Designator_Place.CanPlaceColor ? GhostGraphicValid : GhostGraphicInvalid;

        return result;
    }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        Building_GachaponCollection buildingGachaponCollection;
        if ((buildingGachaponCollection = map.edificeGrid[Position] as Building_GachaponCollection) != null)
        {
            buildingGachaponCollection.Register(this);
        }

        base.SpawnSetup(map, respawningAfterLoad);
    }

    public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
    {
        var map = Map;
        var position = Position;
        Building_GachaponCollection buildingGachaponCollection = null;
        foreach (var thing in map.thingGrid.ThingsAt(position))
        {
            buildingGachaponCollection = thing as Building_GachaponCollection;
            if (buildingGachaponCollection != null)
            {
                break;
            }
        }

        base.DeSpawn(mode);
        if (buildingGachaponCollection == null)
        {
            return;
        }

        buildingGachaponCollection.DeRegister(this);
        map.edificeGrid.InnerArray[map.cellIndices.CellToIndex(position)] = buildingGachaponCollection;
    }
}