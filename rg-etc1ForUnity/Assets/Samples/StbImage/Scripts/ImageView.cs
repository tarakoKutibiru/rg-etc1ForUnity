using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using TarakoKutibiru.RG_ETC1.Runtime;
using Mochineko.StbImageSharpForUnity;
using System.Runtime.InteropServices;
using StbImageSharp;

namespace TarakoKutibiru.RG_ETC1.Samples.StbImage
{
    public class ImageView : MonoBehaviour
    {
        [SerializeField] Renderer source;
        [SerializeField] Renderer commpressed;
        [SerializeField] string filePath;

        void Start()
        {
            StartCoroutine(ShowPicture(filePath));
        }

        IEnumerator ShowPicture(string filePath)
        {
            var path = "file://" + Path.Combine(Application.streamingAssetsPath, filePath);
#if UNITY_ANDROID && !UNITY_EDITOR
            filePath = Path.Combine(Application.streamingAssetsPath, fileName);
#endif
            Debug.Log(path);

            using var request = UnityWebRequest.Get(path);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else {
                var imageResult = ImageDecoder.DecodeImage(request.downloadHandler.data);
                var sourceTexture = imageResult.ToTexture2D();
                source.material.mainTexture = sourceTexture;

                var colors = new Color32[imageResult.Width * imageResult.Height];
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = new Color32(imageResult.Data[i*4], imageResult.Data[i * 4+1], imageResult.Data[i * 4+2], imageResult.Data[i * 4+3]);
                }
                var etc1Data = RgEtc1.EncodeToETC(colors, imageResult.Width, imageResult.Height);
                var commpressedTexture = new Texture2D(imageResult.Width, imageResult.Height, TextureFormat.ETC_RGB4, false);
                commpressedTexture.LoadRawTextureData(etc1Data);
                commpressedTexture.Apply();
                commpressed.material.mainTexture = commpressedTexture;
            }
            request.Dispose();

            yield return null;
        }
    }
}
