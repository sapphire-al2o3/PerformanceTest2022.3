using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class NativeArrayTest : MonoBehaviour
{
    void Start()
    {
        var nativeNrray = new NativeArray<byte>(1000, Allocator.Temp);

        // 初回呼び出しを計測に含めない
        nativeNrray[0] = 0;
        var t = nativeNrray[0];

        using (new ProfilerScope("NativeArray set"))
        {
            for (int i = 0; i < nativeNrray.Length; i++)
            {
                nativeNrray[i] = 0;
            }
        }

        using (new ProfilerScope("NativeArray get"))
        {
            for (int i = 0; i < nativeNrray.Length; i++)
            {
                var v = nativeNrray[i];
            }
        }

        nativeNrray.Dispose();

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
