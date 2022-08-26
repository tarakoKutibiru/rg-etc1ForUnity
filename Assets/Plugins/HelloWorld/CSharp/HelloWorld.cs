using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class HelloWorld  {
#if UNITY_IPHONE
    [DllImport("__Internal")]
    public static extern float FooPluginFunction();
#else
    [DllImport("HelloWorld")]
    public static extern float FooPluginFunction();
#endif
}