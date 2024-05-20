using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;


public class NativeArrayTest : MonoBehaviour
{
    void Start()
    {
        var nativeArray = new NativeArray<byte>(1000, Allocator.Temp);

        // 初回呼び出しを計測に含めない
        nativeArray[0] = 0;
        var t = nativeArray[0];

        using (new ProfilerScope("NativeArray set"))
        {
            for (int i = 0; i < nativeArray.Length; i++)
            {
                nativeArray[i] = 0;
            }
        }

        using (new ProfilerScope("NativeArray get"))
        {
            for (int i = 0; i < nativeArray.Length; i++)
            {
                var v = nativeArray[i];
            }
        }

        nativeArray.Dispose();

        var array = new byte[1000];
        using (new ProfilerScope("Array set"))
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = 0;
            }
        }

        using (new ProfilerScope("Array get"))
        {
            for (int i = 0; i < array.Length; i++)
            {
                var v = array[i];
            }
        }
    }
}
