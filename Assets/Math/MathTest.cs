using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class MathTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 21.51ms
        {
            Profiler.BeginSample("Mathf.CeilToInt");
            int a = 10;
            int b = 7;
            int r = 0;
            for (int i = 0; i < 1000000; i++)
            {
                r = Mathf.CeilToInt((float)a / b);
            }
            Profiler.EndSample();
            Debug.Log(r);
        }

        // 4.18ms
        {
            Profiler.BeginSample("Ceil");
            int a = 10;
            int b = 7;
            int r = 0;
            for (int i = 0; i < 1000000; i++)
            {
                r = (a + b - 1) / b;
            }
            Profiler.EndSample();
            Debug.Log(r);
        }

        // 4.97ms
        {
            Profiler.BeginSample("equal");
            float a = 0;
            float b = 0;
            int r = 0;
            for (int i = 0; i < 1000000; i++)
            {
                if (a == b)
                {
                    r++;
                }
            }
            Profiler.EndSample();
            Debug.Log(r);
        }

        // 92.80ms
        {
            Profiler.BeginSample("Mathf.Approximately");
            float a = 0;
            float b = 0 + Mathf.Epsilon * 2;
            int r = 0;
            for (int i = 0; i < 1000000; i++)
            {
                if (Mathf.Approximately(a, b))
                {
                    r++;
                }
            }
            Profiler.EndSample();
            Debug.Log(r);
        }
    }
}
