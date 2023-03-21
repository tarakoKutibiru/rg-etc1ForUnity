using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using TarakoKutibiru.RG_ETC1.Runtime;

namespace TarakoKutibiru.RG_ETC1.Samples
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

            var request = UnityWebRequestTexture.GetTexture(path);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else {
                var sourceTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                source.material.mainTexture = sourceTexture;

                var stopWatch = new StopWatch();
                stopWatch.Start();
                var commpressedTexture = RgEtc1.EncodeToETC(sourceTexture);
                Debug.Log($"Time: {stopWatch.Stop()}ms");

                commpressed.material.mainTexture = commpressedTexture;
            }

            request.Dispose();

            yield return null;
        }
    }
}
