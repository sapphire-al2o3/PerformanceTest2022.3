using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;


public class NativeArrayTest : MonoBehaviour
{
    void Start()
    {
        const int Max = 100000;
        var nativeArray = new NativeArray<byte>(Max, Allocator.Temp);

        // 初回呼び出しを計測に含めない
        {
            int l = nativeArray.Length;
            nativeArray[0] = 0;
            var t = nativeArray[0];
            var rs = nativeArray.AsReadOnlySpan();
            t = rs[0];
            var s = nativeArray.AsSpan();
            s[0] = 0;
        }

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

        using (new ProfilerScope("NativeArray span get"))
        {
            var span = nativeArray.AsReadOnlySpan();
            for (int i = 0; i < span.Length; i++)
            {
                var v = span[i];
            }
        }

        using (new ProfilerScope("NativeArray span set"))
        {
            var span = nativeArray.AsSpan();
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = 0;
            }
        }

        nativeArray.Dispose();

        var array = new byte[Max];
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
