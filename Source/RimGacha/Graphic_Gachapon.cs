using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGacha
{
	// Token: 0x02000014 RID: 20
	public class Graphic_Gachapon : Graphic
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002D76 File Offset: 0x00000F76
		public override Material MatSingle
		{
			get
			{
				return this.MatSouth;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002D7E File Offset: 0x00000F7E
		public override Material MatWest
		{
			get
			{
				return this.mats[3];
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002D88 File Offset: 0x00000F88
		public override Material MatSouth
		{
			get
			{
				return this.mats[2];
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002D92 File Offset: 0x00000F92
		public override Material MatEast
		{
			get
			{
				return this.mats[1];
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002D9C File Offset: 0x00000F9C
		public override Material MatNorth
		{
			get
			{
				return this.mats[0];
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002DA6 File Offset: 0x00000FA6
		public override bool WestFlipped
		{
			get
			{
				return this.westFlipped;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002DAE File Offset: 0x00000FAE
		public override bool EastFlipped
		{
			get
			{
				return this.eastFlipped;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002DB8 File Offset: 0x00000FB8
		public override bool ShouldDrawRotated
		{
			get
			{
				return (this.data == null || this.data.drawRotated) && (this.MatEast == this.MatNorth || this.MatWest == this.MatNorth);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002E04 File Offset: 0x00001004
		public override float DrawRotatedExtraAngleOffset
		{
			get
			{
				return this.drawRotatedExtraAngleOffset;
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002E0C File Offset: 0x0000100C
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			Texture2D[] array = new Texture2D[this.mats.Length];
			array[0] = ContentFinder<Texture2D>.Get(req.path + "_south", false);
			array[1] = ContentFinder<Texture2D>.Get(req.path + "_west", false);
			array[2] = ContentFinder<Texture2D>.Get(req.path + "_north", false);
			array[3] = ContentFinder<Texture2D>.Get(req.path + "_east", false);
			bool flag = array[0] == null;
			if (flag)
			{
				bool flag2 = array[2] != null;
				if (flag2)
				{
					array[0] = array[2];
					this.drawRotatedExtraAngleOffset = 180f;
				}
				else
				{
					bool flag3 = array[1] != null;
					if (flag3)
					{
						array[0] = array[1];
						this.drawRotatedExtraAngleOffset = -90f;
					}
					else
					{
						bool flag4 = !(array[3] != null);
						if (flag4)
						{
							Log.Error("Failed to find any texture while constructing " + this.ToStringSafe<Graphic_Gachapon>() + ". Filenames have changed; if you are converting an old mod, recommend renaming textures from *_back to *_north, *_side to *_east, and *_front to *_south.", false);
							return;
						}
						array[0] = array[3];
						this.drawRotatedExtraAngleOffset = 90f;
					}
				}
			}
			bool flag5 = array[2] == null;
			if (flag5)
			{
				array[2] = array[0];
			}
			bool flag6 = array[1] == null;
			if (flag6)
			{
				bool flag7 = array[3] != null;
				if (flag7)
				{
					array[1] = array[3];
					this.eastFlipped = base.DataAllowsFlip;
				}
				else
				{
					array[1] = array[0];
				}
			}
			bool flag8 = array[3] == null;
			if (flag8)
			{
				bool flag9 = array[1] != null;
				if (flag9)
				{
					array[3] = array[1];
					this.westFlipped = base.DataAllowsFlip;
				}
				else
				{
					array[3] = array[0];
				}
			}
			float num = (float)array[0].width;
			float num2 = 0f;
			for (int i = 0; i < this.mats.Length; i++)
			{
				Texture2D texture2D = array[i].CreateReadableCopy();
				int[] padding = texture2D.GetPadding(16, 0.04f);
				texture2D.Desatuate();
				texture2D.NormalizeContrastGreyscale(16);
				array[i] = texture2D;
				num = Mathf.Min(new float[]
				{
					num,
					(float)padding[0],
					(float)padding[1],
					(float)padding[2],
					(float)padding[3]
				});
				num2 += (float)texture2D.CountFilledPixel(16);
			}
			num2 = (float)Math.Sqrt((double)(num2 / 4f));
			float num3 = 0.5f * (float)array[0].width / num2;
			this.drawSize *= num3;
			for (int j = 0; j < this.mats.Length; j++)
			{
				MaterialRequest materialRequest = default(MaterialRequest);
				materialRequest.mainTex = array[j];
				materialRequest.shader = req.shader;
				materialRequest.color = this.color;
				materialRequest.colorTwo = this.colorTwo;
				materialRequest.shaderParameters = req.shaderParameters;
				this.mats[j] = MaterialPool.MatFrom(materialRequest);
			}
			GraphicData graphicData = ThingDefOf.Gachapon.graphicData;
			this.baseGraphic = GraphicDatabase.Get<Graphic_Multi>(graphicData.texPath, req.graphicData.shaderType.Shader, graphicData.drawSize, this.color, this.colorTwo, graphicData);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000318C File Offset: 0x0000138C
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			Log.Warning("Graphic_Gachapon.GetColoredVersion(Shader, Color, Color) should not be called. Use Graphic_Gachapon.GetColoredVersion(ShaderTypeDef, Color, Color, ShadowData) instead.", false);
			return GraphicDatabase.Get<Graphic_Gachapon>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000031C4 File Offset: 0x000013C4
		public Graphic GetColoredVersion(ShaderTypeDef newShaderType, Color newColor, Color newColorTwo, ShadowData newShadowData)
		{
			GraphicData graphicData = new GraphicData();
			graphicData.CopyFrom(this.data);
			graphicData.shaderType = newShaderType;
			graphicData.color = newColor;
			graphicData.colorTwo = newColorTwo;
			graphicData.shadowData = newShadowData;
			return graphicData.Graphic;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000320C File Offset: 0x0000140C
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			foreach (Thing thing2 in Find.CurrentMap.thingGrid.ThingsAt(IntVec3Utility.ToIntVec3(loc)))
			{
				bool flag = thing2.def.thingClass == typeof(Building_GachaponCollection);
				if (flag)
				{
					loc.z += 0.1f;
					break;
				}
			}
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			Mesh mesh = this.baseGraphic.MeshAt(rot);
			Quaternion quaternion = base.QuatFromRot(rot);
			bool flag2 = extraRotation != 0f;
			if (flag2)
			{
				quaternion *= Quaternion.Euler(Vector3.up * extraRotation);
			}
			Vector3 vector = new Vector3(loc.x, loc.y - 0.05f, loc.z);
			Graphics.DrawMesh(mesh, loc, quaternion, this.baseGraphic.MatSingle, 0);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000331C File Offset: 0x0000151C
		public override void Print(SectionLayer layer, Thing thing)
		{
			base.Print(layer, thing);
			Vector3 vector = GenThing.TrueCenter(thing);
			vector.y -= 0.05f;
			Printer_Plane.PrintPlane(layer, vector, this.baseGraphic.drawSize, this.baseGraphic.MatSingle, 0f, false, null, null, 0.01f, 0f);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000337C File Offset: 0x0000157C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Gachapon(initPath=",
				this.path,
				", color=",
				this.color,
				", colorTwo=",
				this.colorTwo,
				")"
			});
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000033E0 File Offset: 0x000015E0
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<string>(seed, this.path);
			seed = Gen.HashCombineStruct<Color>(seed, this.color);
			return Gen.HashCombineStruct<Color>(seed, this.colorTwo);
		}

		// Token: 0x04000008 RID: 8
		private Graphic baseGraphic;

		// Token: 0x04000009 RID: 9
		private Material[] mats = new Material[4];

		// Token: 0x0400000A RID: 10
		private bool westFlipped;

		// Token: 0x0400000B RID: 11
		private bool eastFlipped;

		// Token: 0x0400000C RID: 12
		private float drawRotatedExtraAngleOffset;
	}
}
