using System;
using System.Threading.Tasks;
using UnityEngine;
using VRMShaders;
using ColorSpace = VRMShaders.ColorSpace;

namespace TarakoKutibiru.RG_ETC1.Samples.VRM
{
    /// <summary>
    /// Unity ÇÃ ImageConversion.LoadImage ÇópÇ¢Çƒ PNG/JPG ÇÃì«Ç›çûÇ›Çé¿åªÇ∑ÇÈ
    /// </summary>
    public sealed class TextureDeserializer : ITextureDeserializer
    {
        public async Task<Texture2D> LoadTextureAsync(DeserializingTextureInfo textureInfo, IAwaitCaller awaitCaller)
        {
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

            var texture = new Texture2D(2, 2, TextureFormat.ARGB32, textureInfo.UseMipmap, textureInfo.ColorSpace == ColorSpace.Linear);
            if (textureInfo.ImageData != null)
            {
                texture.LoadImage(textureInfo.ImageData);
                texture.wrapModeU = textureInfo.WrapModeU;
                texture.wrapModeV = textureInfo.WrapModeV;
                texture.filterMode = textureInfo.FilterMode;
                await awaitCaller.NextFrame();
            }

            return texture;
        }
    }
}
