using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleScenePresenter : MonoBehaviour
{
    [SerializeField] Text text;

    void Start()
    {
        Debug.Log($"{HelloWorld.FooPluginFunction()}");
        text.text = $"{HelloWorld.FooPluginFunction()}";
    }
}
