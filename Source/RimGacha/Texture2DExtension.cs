using System;
using UnityEngine;
using Verse;

namespace RimGacha
{
	// Token: 0x02000005 RID: 5
	public static class Texture2DExtension
	{
		// Token: 0x06000004 RID: 4 RVA: 0x0000208C File Offset: 0x0000028C
		public static Texture2D CreateReadableCopy(this Texture2D tex)
		{
            RenderTexture temporary = RenderTexture.GetTemporary(tex.width, tex.height, 0, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm, 1);
			Graphics.Blit(tex, temporary);
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = temporary;
			Texture2D texture2D = new Texture2D(tex.width, tex.height);
			texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0);
			texture2D.Apply();
			RenderTexture.active = active;
			RenderTexture.ReleaseTemporary(temporary);
			return texture2D;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002118 File Offset: 0x00000318
		public static void Desatuate(this Texture2D tex)
		{
			Color32[] pixels = tex.GetPixels32();
			int num = tex.height * tex.width;
			for (int i = 0; i < num; i++)
			{
				Color32 color = pixels[i];
				byte b = (byte)Math.Round((double)(((float)color.r + (float)color.g + (float)color.b) / 3f));
				pixels[i] = new Color32(b, b, b, color.a);
			}
			tex.SetPixels32(pixels);
			tex.Apply();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021A4 File Offset: 0x000003A4
		public static void NormalizeContrastGreyscale(this Texture2D tex, byte alphaThreshold = 0)
		{
			Color32[] pixels = tex.GetPixels32();
			int num = tex.height * tex.width;
			int[] array = new int[256];
			int num2 = 0;
			float num3 = 0f;
			for (int i = 0; i < num; i++)
			{
				bool flag = pixels[i].a > alphaThreshold;
				if (flag)
				{
					array[(int)pixels[i].r]++;
					num2++;
					num3 += (float)pixels[i].r;
				}
			}
			num3 /= (float)num2;
			float num4 = GenMath.LerpDouble(255f, 64f, 0.01f, 0.2f, num3);
			int num5 = (int)Math.Round((double)((float)num2 * num4));
			int num6 = 0;
			for (int j = 255; j >= 0; j--)
			{
				num5 -= array[j];
				bool flag2 = num5 < 0;
				if (flag2)
				{
					num6 = j;
					break;
				}
			}
			for (int k = 0; k < num; k++)
			{
				bool flag3 = pixels[k].a > alphaThreshold;
				if (flag3)
				{
					Color32 color = pixels[k];
					byte b = (byte)Math.Round((double)Mathf.Clamp(GenMath.LerpDouble(8f, (float)num6, 0f, 255f, (float)color.r), 0f, 255f));
					pixels[k] = new Color32(b, b, b, color.a);
				}
			}
			tex.SetPixels32(pixels);
			tex.Apply();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002348 File Offset: 0x00000548
		public static int CountFilledPixel(this Texture2D tex, byte alphaThreshold = 0)
		{
			Color32[] pixels = tex.GetPixels32();
			int num = tex.height * tex.width;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				bool flag = pixels[i].a > alphaThreshold;
				if (flag)
				{
					num2++;
				}
			}
			return num2;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000023A4 File Offset: 0x000005A4
		public static int[] GetPadding(this Texture2D tex, byte alphaThreshold = 0, float pixelTolerance = 0f)
		{
			int[] array = new int[4];
			Color32[] pixels = tex.GetPixels32();
			int num = tex.height * tex.width;
			int num2 = 0;
			bool flag = pixelTolerance > 0f;
			if (flag)
			{
				int num3 = 0;
				for (int i = 0; i < num; i++)
				{
					bool flag2 = pixels[i].a > alphaThreshold;
					if (flag2)
					{
						num3++;
					}
				}
				num2 = (int)Math.Round((double)((float)num3 * pixelTolerance));
			}
			int num4 = 0;
			for (int j = 0; j < num; j++)
			{
				bool flag3 = pixels[j].a > alphaThreshold;
				if (flag3)
				{
					num4++;
					bool flag4 = num4 > num2;
					if (flag4)
					{
						array[0] = (int)((float)j / (float)tex.width);
						break;
					}
				}
			}
			num4 = 0;
			for (int k = num - 1; k >= 0; k--)
			{
				int num5 = k % tex.width * tex.height + k / tex.width;
				bool flag5 = pixels[num5].a > alphaThreshold;
				if (flag5)
				{
					num4++;
					bool flag6 = num4 > num2;
					if (flag6)
					{
						array[1] = tex.width - (int)Math.Ceiling((double)((float)num5 % (float)tex.width));
						break;
					}
				}
			}
			num4 = 0;
			for (int l = num - 1; l >= 0; l--)
			{
				bool flag7 = pixels[l].a > alphaThreshold;
				if (flag7)
				{
					num4++;
					bool flag8 = num4 > num2;
					if (flag8)
					{
						array[2] = tex.height - (int)Math.Ceiling((double)((float)l / (float)tex.width));
						break;
					}
				}
			}
			num4 = 0;
			for (int m = 0; m < num; m++)
			{
				int num6 = m % tex.width * tex.height + m / tex.width;
				bool flag9 = pixels[num6].a > alphaThreshold;
				if (flag9)
				{
					num4++;
					bool flag10 = num4 > num2;
					if (flag10)
					{
						array[3] = (int)((float)num6 % (float)tex.width);
						break;
					}
				}
			}
			return array;
		}
	}
}
