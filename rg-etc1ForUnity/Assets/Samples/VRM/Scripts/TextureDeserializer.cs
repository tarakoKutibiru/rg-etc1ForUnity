using System;
using System.Threading.Tasks;
using UnityEngine;
using VRMShaders;
using ColorSpace = VRMShaders.ColorSpace;
using StbImageSharp;
using Mochineko.StbImageSharpForUnity;
using TarakoKutibiru.RG_ETC1;
using TarakoKutibiru.RG_ETC1.Runtime;
using Cysharp.Threading.Tasks;

namespace TarakoKutibiru.RG_ETC1.Samples.VRM
{
    /// <summary>
    /// Unity ÇÃ ImageConversion.LoadImage ÇópÇ¢Çƒ PNG/JPG ÇÃì«Ç›çûÇ›Çé¿åªÇ∑ÇÈ
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

            await UniTask.SwitchToThreadPool();

            var imageResult = ImageDecoder.DecodeImage(textureInfo.ImageData);
            Debug.Log(imageResult.Comp.ToUnityTextureFormat());
            Debug.Log(textureInfo.ImageData.Length);
            Debug.Log(imageResult.Data.Length);
            
            var etc1Data = RgEtc1.EncodeToETC(imageResult.Data, imageResult.Width, imageResult.Height);
            Debug.Log($"etc1Data: {etc1Data}");

            await UniTask.SwitchToMainThread();

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
