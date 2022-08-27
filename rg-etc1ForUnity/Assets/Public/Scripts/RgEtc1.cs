using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TarakoKutibiru.RG_ETC1.Runtime
{
    public static class RgEtc1
    {
    #if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || (UNITY_ANDROID && !UNITY_EDITOR)
        [DllImport("rg_etc1")]
        public static extern void rg_etc1_init();

        [DllImport("rg_etc1", CallingConvention = CallingConvention.Cdecl)]
        public static extern void rg_etc1_pack_etc1_block(ref byte pETC1_block, ref uint pSrc_pixels_rgba, int quality, bool dither);
    #else
        static void rg_etc1_init() {}
        static void rg_etc1_pack_etc1_block(ref byte pETC1_block, ref uint pSrc_pixels_rgba, int quality, bool dither) {}
    #endif

        public static Texture2D encodeETC(Texture2D sourceTexture)
        {
            var width  = sourceTexture.width;
            var height = sourceTexture.height;

            int[] pixels = new int[width * height];
            rg_etc1_init();

            int i, j;

            using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    for (i = 0; i < height; i += 4)
                    {
                        for (j = 0; j < width; j += 4)
                        {
                            int x, y;

                            Color32[] temp = new Color32[16];
                            int       pi   = 0;
                            for (x = i; x < i + 4; x++)
                            {
                                for (y = j; y < j + 4; y++)
                                {
                                    temp[pi++] = sourceTexture.GetPixel(y, x);
                                }
                            }

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

            byte[] result = new byte[8];
            rg_etc1_pack_etc1_block(ref result[0], ref pixels[0], (int)ETC1_Quality.med, false);
            return result;
        }

        public enum ETC1_Quality
        {
            low  = 0,
            med  = 1,
            high = 2
        }
    }
}
