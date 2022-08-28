using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace TarakoKutibiru.RG_ETC1.Samples.NativeArray
{
    public class SampleScenePresenter : MonoBehaviour
    {
        void Start()
        {
            var byteArray = GetNativeArray();
            foreach (var b in byteArray)
            {
                Debug.Log(b);
            }
            byteArray.Dispose();
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
