using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System.Linq.Expressions;
using System.Linq;

public class SetTest : MonoBehaviour
{
    void Start()
    {
        List<int> list0 = new List<int>();
        List<int> list1 = new List<int>();
        for (int i = 1; i <= 1000; i++)
        {
            list0.Add(i);
            if (i % 2 == 0)
            {
                list1.Add(i);
            }
        }

        // 差集合
        // 36.9KB
        {
            Profiler.BeginSample("Linq.Except");
            var ret = list0.Except(list1).ToList();
            Profiler.EndSample();

            Debug.Log(ret.Count);
        }
    }
}
