using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGacha;

public class Graphic_Gachapon : Graphic
{
    private readonly Material[] mats = new Material[4];

    private Graphic baseGraphic;

    private float drawRotatedExtraAngleOffset;

    private bool eastFlipped;

    private bool westFlipped;

    public override Material MatSingle => MatSouth;

    public override Material MatWest => mats[3];

    public override Material MatSouth => mats[2];

    public override Material MatEast => mats[1];

    public override Material MatNorth => mats[0];

    public override bool WestFlipped => westFlipped;

    public override bool EastFlipped => eastFlipped;

    public override bool ShouldDrawRotated =>
        (data == null || data.drawRotated) && (MatEast == MatNorth || MatWest == MatNorth);

    public override float DrawRotatedExtraAngleOffset => drawRotatedExtraAngleOffset;

    public override void Init(GraphicRequest req)
    {
        data = req.graphicData;
        path = req.path;
        color = req.color;
        colorTwo = req.colorTwo;
        drawSize = req.drawSize;
        var array = new Texture2D[mats.Length];
        array[0] = ContentFinder<Texture2D>.Get(req.path + "_south", false);
        array[1] = ContentFinder<Texture2D>.Get(req.path + "_west", false);
        array[2] = ContentFinder<Texture2D>.Get(req.path + "_north", false);
        array[3] = ContentFinder<Texture2D>.Get(req.path + "_east", false);
        if (array[0] == null)
        {
            if (array[2] != null)
            {
                array[0] = array[2];
                drawRotatedExtraAngleOffset = 180f;
            }
            else
            {
                if (array[1] != null)
                {
                    array[0] = array[1];
                    drawRotatedExtraAngleOffset = -90f;
                }
                else
                {
                    if (!(array[3] != null))
                    {
                        Log.Error(
                            "Failed to find any texture while constructing " + this.ToStringSafe() +
                            ". Filenames have changed; if you are converting an old mod, recommend renaming textures from *_back to *_north, *_side to *_east, and *_front to *_south.");
                        return;
                    }

                    array[0] = array[3];
                    drawRotatedExtraAngleOffset = 90f;
                }
            }
        }

        if (array[2] == null)
        {
            array[2] = array[0];
        }

        if (array[1] == null)
        {
            if (array[3] != null)
            {
                array[1] = array[3];
                eastFlipped = DataAllowsFlip;
            }
            else
            {
                array[1] = array[0];
            }
        }

        if (array[3] == null)
        {
            if (array[1] != null)
            {
                array[3] = array[1];
                westFlipped = DataAllowsFlip;
            }
            else
            {
                array[3] = array[0];
            }
        }

        float num = array[0].width;
        var num2 = 0f;
        for (var i = 0; i < mats.Length; i++)
        {
            var texture2D = array[i].CreateReadableCopy();
            var padding = texture2D.GetPadding(16, 0.04f);
            texture2D.Desatuate();
            texture2D.NormalizeContrastGreyscale(16);
            array[i] = texture2D;
            num = Mathf.Min(num, padding[0], padding[1], padding[2], padding[3]);
            num2 += texture2D.CountFilledPixel(16);
        }

        num2 = (float)Math.Sqrt(num2 / 4f);
        var num3 = 0.5f * array[0].width / num2;
        drawSize *= num3;
        for (var j = 0; j < mats.Length; j++)
        {
            var materialRequest = default(MaterialRequest);
            materialRequest.mainTex = array[j];
            materialRequest.shader = req.shader;
            materialRequest.color = color;
            materialRequest.colorTwo = colorTwo;
            materialRequest.shaderParameters = req.shaderParameters;
            mats[j] = MaterialPool.MatFrom(materialRequest);
        }

        var graphicData = ThingDefOf.Gachapon.graphicData;
        baseGraphic = GraphicDatabase.Get<Graphic_Multi>(graphicData.texPath, req.graphicData.shaderType.Shader,
            graphicData.drawSize, color, colorTwo, graphicData);
    }

    public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
    {
        Log.Warning(
            "Graphic_Gachapon.GetColoredVersion(Shader, Color, Color) should not be called. Use Graphic_Gachapon.GetColoredVersion(ShaderTypeDef, Color, Color, ShadowData) instead.");
        return GraphicDatabase.Get<Graphic_Gachapon>(path, newShader, drawSize, newColor, newColorTwo, data);
    }

    public Graphic GetColoredVersion(ShaderTypeDef newShaderType, Color newColor, Color newColorTwo,
        ShadowData newShadowData)
    {
        var graphicData = new GraphicData();
        graphicData.CopyFrom(data);
        graphicData.shaderType = newShaderType;
        graphicData.color = newColor;
        graphicData.colorTwo = newColorTwo;
        graphicData.shadowData = newShadowData;
        return graphicData.Graphic;
    }

    public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
    {
        foreach (var thing2 in Find.CurrentMap.thingGrid.ThingsAt(loc.ToIntVec3()))
        {
            if (thing2.def.thingClass != typeof(Building_GachaponCollection))
            {
                continue;
            }

            loc.z += 0.1f;
            break;
        }

        base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
        var mesh = baseGraphic.MeshAt(rot);
        var quaternion = QuatFromRot(rot);
        if (extraRotation != 0f)
        {
            quaternion *= Quaternion.Euler(Vector3.up * extraRotation);
        }

        var unused = new Vector3(loc.x, loc.y - 0.05f, loc.z);
        Graphics.DrawMesh(mesh, loc, quaternion, baseGraphic.MatSingle, 0);
    }

    public override void Print(SectionLayer layer, Thing thing, float extraRotation)
    {
        base.Print(layer, thing, extraRotation);
        var vector = thing.TrueCenter();
        vector.y -= 0.05f;
        Printer_Plane.PrintPlane(layer, vector, baseGraphic.drawSize, baseGraphic.MatSingle);
    }

    public override string ToString()
    {
        return string.Concat("Gachapon(initPath=", path, ", color=", color, ", colorTwo=", colorTwo, ")");
    }

    public override int GetHashCode()
    {
        var seed = 0;
        seed = Gen.HashCombine(seed, path);
        seed = Gen.HashCombineStruct(seed, color);
        return Gen.HashCombineStruct(seed, colorTwo);
    }
}