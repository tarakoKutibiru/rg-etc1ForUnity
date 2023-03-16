using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TarakoKutibiru.RG_ETC1.Runtime
{
    public static class RgEtc1
    {
    #if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || (UNITY_ANDROID && !UNITY_EDITOR)
        [DllImport("rg_etc1")]
        private static extern void rg_etc1_init();

        [DllImport("rg_etc1", CallingConvention = CallingConvention.Cdecl)]
        private static extern void rg_etc1_pack_etc1_block(ref byte pETC1_block, ref uint pSrc_pixels_rgba, int quality, bool dither);
    #else
        static void rg_etc1_init() {}
        static void rg_etc1_pack_etc1_block(ref byte pETC1_block, ref uint pSrc_pixels_rgba, int quality, bool dither) {}
    #endif

        public static Texture2D EncodeToETC(Texture2D sourceTexture, Quality quality = Quality.Med, bool dithering = false)
        {
            var encodedData   = EncodeToETC(sourceTexture.GetPixels32(), sourceTexture.width, sourceTexture.height, quality, dithering);
            var outputTexture = new Texture2D(sourceTexture.width, sourceTexture.height, TextureFormat.ETC_RGB4, false);
            outputTexture.LoadRawTextureData(encodedData);
            outputTexture.Apply();

            return outputTexture;
        }

        public static byte[] EncodeToETC(Color32[] source, int width, int height, Quality quality = Quality.Med, bool dithering = false)
        {
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

                            Color32[] block = new Color32[16];
                            int       pi    = 0;
                            for (x = i; x < i + 4; x++)
                            {
                                for (y = j; y < j + 4; y++)
                                {
                                    block[pi++] = source[y + x * height];
                                }
                            }

                            writer.Write(EncodeBlock(block, quality, dithering));
                        }
                    }

                    return stream.GetBuffer();
                }
        }

        static byte[] EncodeBlock(Color32[] block, Quality quality, bool dithering)
        {
            uint[] pixels = new uint[block.Length];

            for (int i = 0; i < block.Length; i++)
            {
                pixels[i] = (uint)((block[i].a << 24) | (block[i].b << 16) | (block[i].g << 8) | block[i].r);
            }

            byte[] result = new byte[8];
            rg_etc1_pack_etc1_block(ref result[0], ref pixels[0], (int)quality, dithering);
            return result;
        }
    }
}
