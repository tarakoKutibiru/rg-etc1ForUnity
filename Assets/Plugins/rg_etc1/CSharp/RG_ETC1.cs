using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class RG_ETC1
{
    [DllImport("rg_etc1")]
    public static extern void init();

    [DllImport("rg_etc1", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr encode_etc1(uint[] pSrc_pixels_rgba, int quality, bool dither);

    [DllImport("rg_etc1", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ReleaseMemory(IntPtr ptr);

    public static Texture2D encodeETC(Texture2D sourceTexture)
    {
        var width  = sourceTexture.width;
        var height = sourceTexture.height;

        int[] pixels = new int[width * height];
        init();

        int i, j;

        using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                for (i = 0; i < height; i += 8)
                {
                    for (j = 0; j < width; j += 8)
                    {
                        int x, y;

                        Color32[] temp = new Color32[16];
                        int       pi   = 0;
                        for (x = i; x < i + 4; x++)
                            for (y = j; y < j + 4; y++)
                                temp[pi++] = sourceTexture.GetPixel(y, x);

                        writer.Write(GenETC1(temp));


                        temp = new Color32[16];
                        pi   = 0;
                        for (x = i; x < i + 4; x++)
                            for (y = j + 4; y < j + 8; y++)
                                temp[pi++] = sourceTexture.GetPixel(y, x);

                        writer.Write(GenETC1(temp));


                        temp = new Color32[16];
                        pi   = 0;
                        for (x = i + 4; x < i + 8; x++)
                            for (y = j; y < j + 4; y++)
                                temp[pi++] = sourceTexture.GetPixel(y, x);

                        writer.Write(GenETC1(temp));


                        temp = new Color32[16];
                        pi   = 0;
                        for (x = i + 4; x < i + 8; x++)
                            for (y = j + 4; y < j + 8; y++)
                                temp[pi++] = sourceTexture.GetPixel(y, x);

                        writer.Write(GenETC1(temp));
                    }
                }


                var outputTexture = new Texture2D(width, height, TextureFormat.ETC_RGB4, false);
                outputTexture.LoadRawTextureData(stream.GetBuffer());
                outputTexture.Apply();

                return outputTexture;
            }
    }

    public static byte[] GenETC1(Color32[] colors)
    {
        uint[] pixels = new uint[colors.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            pixels[i] = (uint)((colors[i].a << 24) | (colors[i].b << 16) | (colors[i].g << 8) | colors[i].r);
        }

        IntPtr ptr    = encode_etc1(pixels, (int)ETC1_Quality.med, false);
        byte[] result = new byte[8];
        Marshal.Copy(ptr, result, 0, 8);
        ReleaseMemory(ptr);

        byte[] block = result;    // pack_etc1_block(pixels);

        byte[] ne = new byte[8];
        ne[0] = block[7];
        ne[1] = block[6];
        ne[2] = block[5];
        ne[3] = block[4];
        ne[4] = block[3];
        ne[5] = block[2];
        ne[6] = block[1];
        ne[7] = block[0];

        return ne;
    }

    public enum ETC1_Quality
    {
        low  = 0,
        med  = 1,
        high = 2
    }
}