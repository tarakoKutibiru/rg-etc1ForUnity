using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class SampleScenePresenter : MonoBehaviour
{
    [SerializeField] Renderer source;
    [SerializeField] Renderer commpressed;

    void Start()
    {
        foreach (var b in HelloWorld.GetByteArray())
        {
            Debug.Log(b);
        }

        StartCoroutine(ShowPicture());
    }

    IEnumerator ShowPicture()
    {
        var path = "file://" + Path.Combine(Application.streamingAssetsPath, "cat_256x256.png");
        Debug.Log(path);

        var request = UnityWebRequestTexture.GetTexture(path);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else {
            var sourceTexture      = ((DownloadHandlerTexture)request.downloadHandler).texture;
            var commpressedTexture = RG_ETC1.encodeETC(sourceTexture);

            source.material.mainTexture      = sourceTexture;
            commpressed.material.mainTexture = commpressedTexture;
        }

        request.Dispose();

        yield return null;
    }
}
