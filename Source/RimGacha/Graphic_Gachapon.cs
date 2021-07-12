using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGacha
{
    // Token: 0x02000014 RID: 20
    public class Graphic_Gachapon : Graphic
    {
        // Token: 0x04000009 RID: 9
        private readonly Material[] mats = new Material[4];

        // Token: 0x04000008 RID: 8
        private Graphic baseGraphic;

        // Token: 0x0400000C RID: 12
        private float drawRotatedExtraAngleOffset;

        // Token: 0x0400000B RID: 11
        private bool eastFlipped;

        // Token: 0x0400000A RID: 10
        private bool westFlipped;

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x0600002B RID: 43 RVA: 0x00002D76 File Offset: 0x00000F76
        public override Material MatSingle => MatSouth;

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x0600002C RID: 44 RVA: 0x00002D7E File Offset: 0x00000F7E
        public override Material MatWest => mats[3];

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x0600002D RID: 45 RVA: 0x00002D88 File Offset: 0x00000F88
        public override Material MatSouth => mats[2];

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x0600002E RID: 46 RVA: 0x00002D92 File Offset: 0x00000F92
        public override Material MatEast => mats[1];

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x0600002F RID: 47 RVA: 0x00002D9C File Offset: 0x00000F9C
        public override Material MatNorth => mats[0];

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000030 RID: 48 RVA: 0x00002DA6 File Offset: 0x00000FA6
        public override bool WestFlipped => westFlipped;

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000031 RID: 49 RVA: 0x00002DAE File Offset: 0x00000FAE
        public override bool EastFlipped => eastFlipped;

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x06000032 RID: 50 RVA: 0x00002DB8 File Offset: 0x00000FB8
        public override bool ShouldDrawRotated =>
            (data == null || data.drawRotated) && (MatEast == MatNorth || MatWest == MatNorth);

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x06000033 RID: 51 RVA: 0x00002E04 File Offset: 0x00001004
        public override float DrawRotatedExtraAngleOffset => drawRotatedExtraAngleOffset;

        // Token: 0x06000034 RID: 52 RVA: 0x00002E0C File Offset: 0x0000100C
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
                num = Mathf.Min(num, padding[0], (float) padding[1], (float) padding[2], (float) padding[3]);
                num2 += texture2D.CountFilledPixel(16);
            }

            num2 = (float) Math.Sqrt(num2 / 4f);
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

        // Token: 0x06000035 RID: 53 RVA: 0x0000318C File Offset: 0x0000138C
        public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
        {
            Log.Warning(
                "Graphic_Gachapon.GetColoredVersion(Shader, Color, Color) should not be called. Use Graphic_Gachapon.GetColoredVersion(ShaderTypeDef, Color, Color, ShadowData) instead.");
            return GraphicDatabase.Get<Graphic_Gachapon>(path, newShader, drawSize, newColor, newColorTwo, data);
        }

        // Token: 0x06000036 RID: 54 RVA: 0x000031C4 File Offset: 0x000013C4
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

        // Token: 0x06000037 RID: 55 RVA: 0x0000320C File Offset: 0x0000140C
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

        // Token: 0x06000038 RID: 56 RVA: 0x0000331C File Offset: 0x0000151C
        public override void Print(SectionLayer layer, Thing thing, float extraRotation)
        {
            base.Print(layer, thing, extraRotation);
            var vector = thing.TrueCenter();
            vector.y -= 0.05f;
            Printer_Plane.PrintPlane(layer, vector, baseGraphic.drawSize, baseGraphic.MatSingle);
        }

        // Token: 0x06000039 RID: 57 RVA: 0x0000337C File Offset: 0x0000157C
        public override string ToString()
        {
            return string.Concat("Gachapon(initPath=", path, ", color=", color, ", colorTwo=", colorTwo, ")");
        }

        // Token: 0x0600003A RID: 58 RVA: 0x000033E0 File Offset: 0x000015E0
        public override int GetHashCode()
        {
            var seed = 0;
            seed = Gen.HashCombine(seed, path);
            seed = Gen.HashCombineStruct(seed, color);
            return Gen.HashCombineStruct(seed, colorTwo);
        }
    }
}