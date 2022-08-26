using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class HelloWorld : MonoBehaviour {

    [SerializeField] Text text;

#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern float FooPluginFunction();
#else
    [DllImport("HelloWorld")]
    private static extern float FooPluginFunction();
#endif

    void Start()
    {
        this.text.text = $"HelloWorld.FooPluginFunction() {FooPluginFunction()}";
    }
}