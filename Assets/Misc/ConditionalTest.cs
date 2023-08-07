using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class ConditionalTest : MonoBehaviour
{
    bool IsZero(int i)
    {
        return i == 0;
    }

    bool IsZero2(int i)
    {
        return i == 0 ? true : false;
    }

    void Start()
    {
        {
            Profiler.BeginSample("conditional");
            for (int i = 0; i < 100000; i++)
            {
                bool b = IsZero(i);
            }
            Profiler.EndSample();
        }

        // ?:‚ðŽg‚Á‚½‚Ù‚¤‚ªŽáŠ±’x‚­‚È‚é
        {
            Profiler.BeginSample("conditional2");
            for (int i = 0; i < 100000; i++)
            {
                bool b = IsZero2(i);
            }
            Profiler.EndSample();
        }
    }
}
