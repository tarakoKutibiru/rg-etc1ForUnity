using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;
using TarakoKutibiru.RG_ETC1.Runtime;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System.Threading;

namespace TarakoKutibiru.RG_ETC1.Samples
{
    public class SampleScenePresenter : MonoBehaviour
    {
        [SerializeField] Renderer source;
        [SerializeField] Renderer commpressed;

        [SerializeField] List<string> targetImageNameArray = new List<string>() {};
        Stopwatch stopwatch = new Stopwatch();

        async void Start()
        {
            await ShowPictures(targetImageNameArray.ToArray());
        }

        string NameToPath(string fileName)
        {
        #if UNITY_ANDROID && !UNITY_EDITOR
            return Path.Combine(Application.streamingAssetsPath, fileName);
        #else
            return "file://" + Path.Combine(Application.streamingAssetsPath, fileName);
        #endif
        }

        async UniTask ShowPictures(string[] fileNameArray)
        {
            stopwatch.Reset();

            foreach (var fileName in fileNameArray)
            {
                ResetPicture();
                await ShowPicture(NameToPath(fileName));
                await UniTask.Delay(TimeSpan.FromMilliseconds(500));
            }
        }

        async UniTask ShowPicture(string path)
        {
            var request = UnityWebRequestTexture.GetTexture(path);
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else {
                var sourceTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                source.material.mainTexture = sourceTexture;

                stopwatch.Reset();
                stopwatch.Start();

                var pixelDataArray = sourceTexture.GetPixels32();
                var width          = sourceTexture.width;
                var height         = sourceTexture.height;

                await UniTask.SwitchToThreadPool();
                var encodedPixelDataArray = RgEtc1.EncodeToETC(pixelDataArray, width, height);

                await UniTask.SwitchToMainThread();
                var encodedTexture = new Texture2D(width, height, TextureFormat.ETC_RGB4, false);
                encodedTexture.LoadRawTextureData(encodedPixelDataArray);
                encodedTexture.Apply();
                stopwatch.Stop();

                Debug.Log($"{Path.GetFileName(path)} Time: {stopwatch.Elapsed.Milliseconds} ms");
                commpressed.material.mainTexture = encodedTexture;
            }

            request.Dispose();
        }

        void ResetPicture()
        {
            if (source.material.mainTexture != null)
            {
                var prevTexture = source.material.mainTexture;
                source.material.mainTexture = null;
                Destroy(prevTexture);
            }

            if (commpressed.material.mainTexture != null)
            {
                var prevTexture = commpressed.material.mainTexture;
                commpressed.material.mainTexture = null;
                Destroy(prevTexture);
            }
        }
    }
}
