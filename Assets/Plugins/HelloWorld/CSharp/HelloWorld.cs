using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;

public class HelloWorld  {
#if UNITY_IPHONE
    [DllImport("__Internal")]
    public static extern float FooPluginFunction();
#else
    [DllImport("HelloWorld")]
    public static extern float FooPluginFunction();

    [DllImport("HelloWorld")]
    static extern void helloworld_get_int_array(IntPtr ptr);
#endif

    public static int[] GetIntArray()
    {
        int    arraySize = Marshal.SizeOf(typeof(int)) * 8;
        IntPtr intPtr    = Marshal.AllocCoTaskMem(arraySize);

        helloworld_get_int_array(intPtr);

        int[] intArray = new int[8];
        Marshal.Copy(intPtr, intArray, 0, 8);

        return intArray;
    }
}