using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class TupleTest : MonoBehaviour
{
    (int min, int max) GetMinMax(int x0, int x1, int x2)
    {
        int min = 0;
        int max = 0;
        if (x0 > x1)
        {
            if (x2 > x0)
            {
                max = x2;
            }
            else
            {
                max = x0;
            }
            if (x2 > x1)
            {
                min = x1;
            }
            else
            {
                min = x2;
            }
        }
        else
        {
            if (x2 > x1)
            {
                max = x2;
            }
            else
            {
                max = x1;
            }
            if (x2 > x0)
            {
                min = x0;
            }
            else
            {
                min = x2;
            }
        }

        return (min, max);
    }

    void Start()
    {
        {
            Profiler.BeginSample("Tuple");
            (int x, int y) p = (1, 2);
            Profiler.EndSample();

            Debug.Log($"{p.x},{p.y}");
        }

        {
            Profiler.BeginSample("Tuple return");
            var (min, max) = GetMinMax(5, 1, 3);
            Profiler.EndSample();

            Debug.Log($"{min},{max}");
        }
    }
}
