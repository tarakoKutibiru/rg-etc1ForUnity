using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniGLTF;
using System.IO;
using System.Diagnostics;
using TarakoKutibiru.RG_ETC1.Runtime;
using UnityEngine.Networking.Types;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System;
using VRMShaders;

namespace TarakoKutibiru.RG_ETC1.Samples.VRM
{
    public class VRMView : MonoBehaviour
    {
        [SerializeField] string filePath = string.Empty;
        [SerializeField] bool etc;

        // Start is called before the first frame update
        async UniTask Start()
        {
            var path = "file://" + Path.Combine(Application.streamingAssetsPath, filePath);
#if UNITY_ANDROID && !UNITY_EDITOR
            path = Path.Combine(Application.streamingAssetsPath, fileName);
#endif
            await this.LoadVRM(path);
        }

        async UniTask LoadVRM(string path)
        {
            Debug.Log(path);

            using var request = UnityWebRequest.Get(path);
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                var awaitCaller = new ImmediateCaller();
                var data = new UniGLTF.GlbBinaryParser(request.downloadHandler.data,this.gameObject.name).Parse();
                using var context = new UniGLTF.ImporterContext(data, textureDeserializer: etc ? new TextureDeserializer():null);
                var instance = await context.LoadAsync(awaitCaller);
                instance.ShowMeshes();
                instance.transform.parent = this.transform;
                instance.transform.localPosition = Vector3.zero;
            }
        }
    }
}
