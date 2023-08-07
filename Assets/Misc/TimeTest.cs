using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class TimeTest : MonoBehaviour
{
    void Start()
    {
        {
            Profiler.BeginSample("Now");
            for (int i = 0; i < 100; i++)
            {
                var time = System.DateTime.Now;
            }
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("UtcNow");
            for (int i = 0; i < 100; i++)
            {
                var time = System.DateTime.UtcNow;
            }
            Profiler.EndSample();
        }
    }
}
