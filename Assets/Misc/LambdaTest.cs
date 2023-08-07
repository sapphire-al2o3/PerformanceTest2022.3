using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Profiling;

public class LambdaTest : MonoBehaviour
{

    int Func0(int x)
    {
        return x * 2;
    }

    int Func1(int x) => x * 2;

    Func<int, int> Func2 = x => x * 2;

    int Call(int x, Func<int, int> func)
    {
        return func(x);
    }

    List<int> list = new List<int>();

    List<int> RemoveList0(List<int> list, int v)
    {
        if (list != null && list.Count > 0)
        {
            return list.FindAll(x => x != v);
        }
        return null;
    }

    List<int> RemoveList1(List<int> list, int v)
    {
        if (list != null && list.Count > 0)
        {
            int t = v;
            return list.FindAll(x => x != t);
        }
        return null;
    }

	System.Func<int, int> func;

    void Start()
    {
        // 10.9KB
        {
            int k = 0;
            Profiler.BeginSample("function 0");
            for (int i = 0; i < 100; i++)
            {
                k += Call(i, Func0);
            }
            Profiler.EndSample();
            Debug.Log(k);
        }

        // 10.9KB
        {
            int k = 0;
            Profiler.BeginSample("function 1");
            for (int i = 0; i < 100; i++)
            {
                k += Call(i, Func1);
            }
            Profiler.EndSample();
            Debug.Log(k);
        }

        // 0B
        {
            int k = 0;
            Profiler.BeginSample("function 2");
            for (int i = 0; i < 100; i++)
            {
                k += Call(i, Func2);
            }
            Profiler.EndSample();
            Debug.Log(k);
        }

        // 112B
        {
            int k = 0;
            Profiler.BeginSample("function 3");
            for (int i = 0; i < 100; i++)
            {
                k += Call(i, x => x * 2);
            }
            Profiler.EndSample();
            Debug.Log(k);
        }

        // 11KB
        {
            int k = 0;
            Profiler.BeginSample("function 4");
            for (int i = 0; i < 100; i++)
            {
                k += Call(i, x => i * 2);
            }
            Profiler.EndSample();
            Debug.Log(k);
        }

        // 2.0KB
        {
            Profiler.BeginSample("capture test 0");
            for (int i = 0; i < 100; i++)
            {
                RemoveList0(null, i);
            }
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("capture test 1");
            for (int i = 0; i < 100; i++)
            {
                RemoveList1(null, i);
            }
            Profiler.EndSample();
        }

        // 32byte
        {
            Profiler.BeginSample("anonymous type");
            var a = new
            {
                A = 100,
                B = "hoge"
            };
            int i = a.A;
            string j = a.B;

            Profiler.EndSample();
        }

        // 112byte
        {
            Profiler.BeginSample("add");
            func += Func0;
            Profiler.EndSample();
        }

        // 272byte
        {
            Profiler.BeginSample("add 2");
            func += Func1;
            Profiler.EndSample();
            func -= Func1;
        }

        // 152byte
        {
            Profiler.BeginSample("remove");
            func -= Func0;
            Profiler.EndSample();
        }
    }
}
