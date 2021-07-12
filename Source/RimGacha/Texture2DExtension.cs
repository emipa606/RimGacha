using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Verse;

namespace RimGacha
{
    // Token: 0x02000005 RID: 5
    public static class Texture2DExtension
    {
        // Token: 0x06000004 RID: 4 RVA: 0x0000208C File Offset: 0x0000028C
        public static Texture2D CreateReadableCopy(this Texture2D tex)
        {
            var temporary = RenderTexture.GetTemporary(tex.width, tex.height, 0, GraphicsFormat.R8G8B8A8_UNorm, 1);
            Graphics.Blit(tex, temporary);
            var active = RenderTexture.active;
            RenderTexture.active = temporary;
            var texture2D = new Texture2D(tex.width, tex.height);
            texture2D.ReadPixels(new Rect(0f, 0f, temporary.width, temporary.height), 0, 0);
            texture2D.Apply();
            RenderTexture.active = active;
            RenderTexture.ReleaseTemporary(temporary);
            return texture2D;
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002118 File Offset: 0x00000318
        public static void Desatuate(this Texture2D tex)
        {
            var pixels = tex.GetPixels32();
            var num = tex.height * tex.width;
            for (var i = 0; i < num; i++)
            {
                var color = pixels[i];
                var b = (byte) Math.Round((color.r + (float) color.g + color.b) / 3f);
                pixels[i] = new Color32(b, b, b, color.a);
            }

            tex.SetPixels32(pixels);
            tex.Apply();
        }

        // Token: 0x06000006 RID: 6 RVA: 0x000021A4 File Offset: 0x000003A4
        public static void NormalizeContrastGreyscale(this Texture2D tex, byte alphaThreshold = 0)
        {
            var pixels = tex.GetPixels32();
            var num = tex.height * tex.width;
            var array = new int[256];
            var num2 = 0;
            var num3 = 0f;
            for (var i = 0; i < num; i++)
            {
                if (pixels[i].a <= alphaThreshold)
                {
                    continue;
                }

                array[pixels[i].r]++;
                num2++;
                num3 += pixels[i].r;
            }

            num3 /= num2;
            var num4 = GenMath.LerpDouble(255f, 64f, 0.01f, 0.2f, num3);
            var num5 = (int) Math.Round(num2 * num4);
            var num6 = 0;
            for (var j = 255; j >= 0; j--)
            {
                num5 -= array[j];
                if (num5 >= 0)
                {
                    continue;
                }

                num6 = j;
                break;
            }

            for (var k = 0; k < num; k++)
            {
                if (pixels[k].a <= alphaThreshold)
                {
                    continue;
                }

                var color = pixels[k];
                var b = (byte) Math.Round(Mathf.Clamp(GenMath.LerpDouble(8f, num6, 0f, 255f, color.r), 0f, 255f));
                pixels[k] = new Color32(b, b, b, color.a);
            }

            tex.SetPixels32(pixels);
            tex.Apply();
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002348 File Offset: 0x00000548
        public static int CountFilledPixel(this Texture2D tex, byte alphaThreshold = 0)
        {
            var pixels = tex.GetPixels32();
            var num = tex.height * tex.width;
            var num2 = 0;
            for (var i = 0; i < num; i++)
            {
                if (pixels[i].a > alphaThreshold)
                {
                    num2++;
                }
            }

            return num2;
        }

        // Token: 0x06000008 RID: 8 RVA: 0x000023A4 File Offset: 0x000005A4
        public static int[] GetPadding(this Texture2D tex, byte alphaThreshold = 0, float pixelTolerance = 0f)
        {
            var array = new int[4];
            var pixels = tex.GetPixels32();
            var num = tex.height * tex.width;
            var num2 = 0;
            if (pixelTolerance > 0f)
            {
                var num3 = 0;
                for (var i = 0; i < num; i++)
                {
                    if (pixels[i].a > alphaThreshold)
                    {
                        num3++;
                    }
                }

                num2 = (int) Math.Round(num3 * pixelTolerance);
            }

            var num4 = 0;
            for (var j = 0; j < num; j++)
            {
                if (pixels[j].a <= alphaThreshold)
                {
                    continue;
                }

                num4++;
                if (num4 <= num2)
                {
                    continue;
                }

                array[0] = (int) (j / (float) tex.width);
                break;
            }

            num4 = 0;
            for (var k = num - 1; k >= 0; k--)
            {
                var num5 = (k % tex.width * tex.height) + (k / tex.width);
                if (pixels[num5].a <= alphaThreshold)
                {
                    continue;
                }

                num4++;
                if (num4 <= num2)
                {
                    continue;
                }

                array[1] = tex.width - (int) Math.Ceiling(num5 % (float) tex.width);
                break;
            }

            num4 = 0;
            for (var l = num - 1; l >= 0; l--)
            {
                if (pixels[l].a <= alphaThreshold)
                {
                    continue;
                }

                num4++;
                if (num4 <= num2)
                {
                    continue;
                }

                array[2] = tex.height - (int) Math.Ceiling(l / (float) tex.width);
                break;
            }

            num4 = 0;
            for (var m = 0; m < num; m++)
            {
                var num6 = (m % tex.width * tex.height) + (m / tex.width);
                if (pixels[num6].a <= alphaThreshold)
                {
                    continue;
                }

                num4++;
                if (num4 <= num2)
                {
                    continue;
                }

                array[3] = (int) (num6 % (float) tex.width);
                break;
            }

            return array;
        }
    }
}