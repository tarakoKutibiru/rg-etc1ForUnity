using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using TarakoKutibiru.RG_ETC1.Runtime;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace TarakoKutibiru.RG_ETC1.Samples.NativeArray
{
    public class SampleScenePresenter : MonoBehaviour
    {
        [SerializeField] Renderer     commpressed;
        [SerializeField] List<string> targetImageNameList = new List<string>() {};


        Stopwatch stopwatch = new Stopwatch();

        async void Start()
        {
            await ShowPictures(targetImageNameList.ToArray());
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
            var sourceTexture = await this.LoadTexture2D(path);

            var colorNativeArray = sourceTexture.GetRawTextureData<Color32>();
            var width            = sourceTexture.width;
            var height           = sourceTexture.height;

            // await UniTask.SwitchToThreadPool();
            var encodedColorNativeArray = new NativeArray<byte>();

            stopwatch.Reset();
            stopwatch.Start();
            RgEtc1.EncodeToETC(ref encodedColorNativeArray, ref colorNativeArray, width, height);
            Debug.Log($"{Path.GetFileName(path)} Time: {stopwatch.Elapsed.Milliseconds} ms");
            // await UniTask.SwitchToMainThread();

            var encodedTexture = new Texture2D(width, height, TextureFormat.ETC_RGB4, false);
            encodedTexture.LoadRawTextureData(encodedColorNativeArray);
            encodedTexture.Apply();

            commpressed.material.mainTexture = encodedTexture;

            Destroy(sourceTexture);
            colorNativeArray.Dispose();
            encodedColorNativeArray.Dispose();
        }

        string NameToPath(string fileName)
        {
        #if UNITY_ANDROID && !UNITY_EDITOR
            return Path.Combine(Application.streamingAssetsPath, fileName);
        #else
            return "file://" + Path.Combine(Application.streamingAssetsPath, fileName);
        #endif
        }

        async UniTask<Texture2D> LoadTexture2D(string path)
        {
            using (var request = UnityWebRequestTexture.GetTexture(path))
            {
                await request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(request.error);
                    return null;
                }
                else {
                    return ((DownloadHandlerTexture)request.downloadHandler).texture;;
                }
            }
        }

        void ResetPicture()
        {
            if (commpressed.material.mainTexture != null)
            {
                var prevTexture = commpressed.material.mainTexture;
                commpressed.material.mainTexture = null;
                Destroy(prevTexture);
            }
        }

        public unsafe NativeArray<byte> GetNativeArray()
        {
            var nativeByteArray = new NativeArray<byte>(8, Allocator.Persistent);
            var ptr             = (IntPtr)nativeByteArray.GetUnsafePtr();
            HelloWorld.helloworld_get_native_array(ptr);
            return nativeByteArray;
        }
    }
}
