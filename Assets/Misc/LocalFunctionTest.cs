using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class LocalFunctionTest : MonoBehaviour
{
    void Start()
    {
        // 0byte
        {
            Profiler.BeginSample("LocalFunction");
            int Add(int a, int b)
            {
                return a + b;
            }

            int sum = Add(10, 20);

            Profiler.EndSample();
        }
    }
}
