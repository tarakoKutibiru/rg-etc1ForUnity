using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;

public class HelloWorld  {
#if UNITY_IPHONE
    [DllImport("__Internal")]
    public static extern float FooPluginFunction();
#elif UNITY_OSX || UNITY_ANDROID
    
    #region Log
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DebugLogDelegate(string str);
    DebugLogDelegate debugLogFunc = msg => Debug.Log(msg);
    
    [DllImport("HelloWorld")] public static extern void helloworld_set_debug_log_func(IntPtr ptr);
    [DllImport("HelloWorld")] public static extern void helloworld_debug_log_test();
    #endregion

    [DllImport("HelloWorld")]
    public static extern float FooPluginFunction();

    [DllImport("HelloWorld")]
    static extern void helloworld_get_byte(ref byte ptr);

    [DllImport("HelloWorld")]
    static extern void helloworld_get_byte_array(ref byte ptr);

    [DllImport("HelloWorld")]
    static extern void helloworld_get_int_array(IntPtr ptr);
#else
    public static float FooPluginFunction() { return 0.0f; }
    static void helloworld_get_byte(ref byte ptr) {}
    static void helloworld_get_byte_array(ref byte ptr) {}
    static void helloworld_get_int_array(IntPtr ptr) {}
#endif

    public void Init()
    {
        #if UNITY_OSX || UNITY_ANDROID
        var callback = new DebugLogDelegate(debugLogFunc);
        var ptr = Marshal.GetFunctionPointerForDelegate(callback);
        helloworld_set_debug_log_func(ptr);
        helloworld_debug_log_test();
        #endif
    }

    public static byte GetByte()
    {
        byte b = new byte();
        helloworld_get_byte(ref b);
        return b;
    }

    public static byte[] GetByteArray()
    {
        byte[] b = new byte[8];
        helloworld_get_byte_array(ref b[0]);
        return b;
    }

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