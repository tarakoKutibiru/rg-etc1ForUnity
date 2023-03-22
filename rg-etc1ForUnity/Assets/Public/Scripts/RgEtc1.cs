using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
            rg_etc1_init();
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            Parallel.For(0, height / 4, i =>
            {
                for (int j = 0; j < width; j += 4)
                {
                    Color32[] block = new Color32[16];
                    int pi = 0;
                    for (int x = i * 4; x < i * 4 + 4; x++)
                    {
                        for (int y = j; y < j + 4; y++)
                        {
                            block[pi++] = source[y + x * height];
                        }
                    }
                    byte[] encodedBlock = EncodeBlock(block, quality, dithering);
                    lock (writer)
                    {
                        writer.Write(encodedBlock);
                    }
                }
            });
            return stream.ToArray();
        }

        static byte[] EncodeBlock(Color32[] block, Quality quality, bool dithering)
        {
            uint[] pixels = block.Select(color => (uint)((color.a << 24) | (color.b << 16) | (color.g << 8) | color.r)).ToArray();
            byte[] result = new byte[8];
            rg_etc1_pack_etc1_block(ref result[0], ref pixels[0], (int)quality, dithering);
            return result;
        }

        public static byte[] EncodeToETC(byte[] source, int width, int height, Quality quality = Quality.Med, bool dithering = false)
        {
            rg_etc1_init();

            int i, j;

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            for (i = 0; i < height; i += 4)
            {
                for (j = 0; j < width; j += 4)
                {
                    int x, y;

                    var block = new byte[16*4];
                    int pi = 0;
                    for (x = i; x < i + 4; x++)
                    {
                        for (y = j; y < j + 4; y++)
                        {
                            block[pi*4]   = source[(y + x * height)*4];
                            block[pi*4+1] = source[(y + x * height) * 4+1];
                            block[pi*4+2] = source[(y + x * height) * 4+2];
                            block[pi*4+3] = source[(y + x * height) * 4+3];

                            pi++;
                        }
                    }

                    writer.Write(EncodeBlock(block, quality, dithering));
                }
            }

            return stream.GetBuffer();
        }

        static byte[] EncodeBlock(byte[] block, Quality quality, bool dithering)
        {
            uint[] pixels = new uint[4*4];

                        for (int i = 0; i < pixels.Length; i++)
                        {
                            pixels[i] = (uint)((block[i*4+3] << 24) | (block[i * 4 + 2] << 16) | (block[i * 4 + 1] << 8) | block[i * 4]); ;
                        }

                        byte[] result = new byte[8];
                        rg_etc1_pack_etc1_block(ref result[0], ref pixels[0], (int)quality, dithering);
            return result;
        }
    }
}
