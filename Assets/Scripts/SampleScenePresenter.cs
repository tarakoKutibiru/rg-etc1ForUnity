using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class SampleScenePresenter : MonoBehaviour
{
    [SerializeField] Renderer renderer;

    void Start()
    {
        StartCoroutine(ShowPicture());
    }

    IEnumerator ShowPicture()
    {
        var path = "file://" + Path.Combine(Application.streamingAssetsPath, "cat.png");
        Debug.Log(path);

        var request = UnityWebRequestTexture.GetTexture(path);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else {
            var sourceTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            renderer.material.mainTexture = sourceTexture;
        }

        request.Dispose();

        yield return null;
    }
}
