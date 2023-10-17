using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System;

public class SpanTest : MonoBehaviour
{
    void Start()
    {
        // メソッド終了時にスタックから消える(スコープと違う)
        // IL2CPPではallocaが呼び出される
        // 0byte
        {
            Profiler.BeginSample("stackalloc");
            Span<int> span = stackalloc int[10];
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = i;
            }
            Profiler.EndSample();
        }
    }
}
