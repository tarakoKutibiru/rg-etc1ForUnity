using System;
using System.Threading.Tasks;
using UnityEngine;
using VRMShaders;
using ColorSpace = VRMShaders.ColorSpace;
using StbImageSharp;
using Mochineko.StbImageSharpForUnity;
using TarakoKutibiru.RG_ETC1;
using TarakoKutibiru.RG_ETC1.Runtime;

namespace TarakoKutibiru.RG_ETC1.Samples.VRM
{
    /// <summary>
    /// Unity の ImageConversion.LoadImage を用いて PNG/JPG の読み込みを実現する
    /// </summary>
    public sealed class TextureDeserializer : ITextureDeserializer
    {
        public async Task<Texture2D> LoadTextureAsync(DeserializingTextureInfo textureInfo, IAwaitCaller awaitCaller)
        {
            Debug.Log($"###Start");
            switch (textureInfo.DataMimeType)
            {
                case "image/png":
                    break;
                case "image/jpeg":
                    break;
                default:
                    if (string.IsNullOrEmpty(textureInfo.DataMimeType))
                    {
                        Debug.Log($"Texture image MIME type is empty.");
                    }
                    else
                    {
                        Debug.Log($"Texture image MIME type `{textureInfo.DataMimeType}` is not supported.");
                    }
                    break;
            }

            var imageResult = ImageDecoder.DecodeImage(textureInfo.ImageData);
            Debug.Log(imageResult.Comp.ToUnityTextureFormat());
            Debug.Log(textureInfo.ImageData.Length);
            Debug.Log(imageResult.Data.Length);
            
            var colors = new Color32[imageResult.Width*imageResult.Height];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color32(imageResult.Data[i * 4], imageResult.Data[i * 4 + 1], imageResult.Data[i * 4 + 2], imageResult.Data[i * 4 + 3]);
            }
            var etc1Data = RgEtc1.EncodeToETC(colors, imageResult.Width, imageResult.Height);
            Debug.Log($"etc1Data: {etc1Data}");

            var texture = new Texture2D(imageResult.Width, imageResult.Height, TextureFormat.ETC_RGB4, textureInfo.UseMipmap, textureInfo.ColorSpace == ColorSpace.Linear);
            texture.LoadRawTextureData(etc1Data);
            texture.wrapModeU = textureInfo.WrapModeU;
            texture.wrapModeV = textureInfo.WrapModeV;
            texture.filterMode = textureInfo.FilterMode;
            texture.Apply();
            Debug.Log($"###Done");

            return texture;
        }
    }
}
