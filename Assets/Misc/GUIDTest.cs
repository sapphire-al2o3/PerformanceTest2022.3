using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System;

public class GUIDTest : MonoBehaviour
{
    void Start()
    {
        {
            // 48byte
            Profiler.BeginSample("NewGuid");
            var guid = Guid.NewGuid();
            Profiler.EndSample();

            // 90byte + 48byte
            Profiler.BeginSample("Guid.ToString");
            var text = guid.ToString("N");
            Profiler.EndSample();
            
            // 48byte
            Profiler.BeginSample("Guid.ToByteArray");
            var bytes = guid.ToByteArray();
            Profiler.EndSample();
        }
    }
}
