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

namespace TarakoKutibiru.RG_ETC1.Samples.VRM
{
    public class Presenter : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var path = "file://" + Path.Combine(Application.streamingAssetsPath, "Alicia_VRM", "Alicia", "VRM", "AliciaSolid.vrm");
#if UNITY_ANDROID && !UNITY_EDITOR
            path = Path.Combine(Application.streamingAssetsPath, fileName);
#endif
            StartCoroutine(this.LoadVRM(path));

        }

        IEnumerator LoadVRM(string path)
        {

            Debug.Log(path);

            using var request = UnityWebRequest.Get(path);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                var data = new UniGLTF.GlbBinaryParser(request.downloadHandler.data, "AliciaSolid").Parse();
                using var context = new UniGLTF.ImporterContext(data);
                var instance = context.Load();
                instance.ShowMeshes();
            }

            yield return null;
        }
    }
}
