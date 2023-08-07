using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Profiling;

public class AsyncTest : MonoBehaviour
{
    void Start()
    {
        // 19.0KB
        {
            Profiler.BeginSample("Async");
            Async0();
            Profiler.EndSample();
        }

        int i = 10;
        var s = nameof(i);
        Debug.Log(s);
    }

    async void Async0()
    {
        await Task.Delay(10);
        Debug.Log($"End: {Time.frameCount}");
    }
}
