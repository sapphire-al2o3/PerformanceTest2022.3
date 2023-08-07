using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class InputTest : MonoBehaviour
{
    void Start()
    {
        // 80byte
        Profiler.BeginSample("Input.touches");
        foreach (var touch in Input.touches)
        {
        }
        Profiler.EndSample();

        // 0byte
        Profiler.BeginSample("Input.GetTouch");
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
        }
        Profiler.EndSample();
    }
}
