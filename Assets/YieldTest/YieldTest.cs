using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class YieldTest : MonoBehaviour
{
    IEnumerator Test()
    {
        int i = 0;

        while (true)
        {
            {
                Matrix4x4 a = Matrix4x4.identity;
                Matrix4x4 b = Matrix4x4.identity;
                Matrix4x4 c = Matrix4x4.identity;
                Matrix4x4 d = Matrix4x4.identity;
                Matrix4x4 e = Matrix4x4.identity;

                i += 1;
            }
            yield return null;
        }
    }

    IEnumerator Test0()
    {
        while (true)
        {
            yield return null;
        }
    }

    IEnumerator Test1()
    {
        while (true)
        {
            // 20byte
            yield return 0;
        }
    }

    int[] array = new int[1024];
    IEnumerator Test2()
    {
        int count = array.Length;
        int s = 0;
        for (int i = 0; i < count; i++)
        {
            int a = array[i];
            s += a;
            yield return null;
        }
    }

    IEnumerator Test3()
    {
        int s = 0;
        for (int i = 0; i < array.Length; i++)
        {
            s += array[i];
            yield return null;
        }
    }

    IEnumerator Test4()
    {
        int s = 0;
        foreach (var a in array)
        {
            s += a;
            yield return null;
        }
    }

    IEnumerator Test5(int[] array)
    {
        int s = 0;
        for (int i = 0; i < array.Length; i++)
        {
            s += array[i];
            yield return null;
        }
    }

    IEnumerable<int> Even(int max)
    {
        for (int i = 1; i <= max; i++)
        {
            if (i % 2 == 0)
            {
                yield return i;
            }
        }
        yield break;
    }

    public struct OddEnumerator
    {
        public OddEnumerator(int max)
        {
            _max = max;
            _index = 1;
            _current = _index;
        }
        int _index;
        int _max;
        int _current;

        public bool MoveNext()
        {
            for (; _index <= _max; _index++)
            {
                if (_index % 2 == 1)
                {
                    _current = _index;
                    _index++;
                    return true;
                }
            }
            return false;
        }
        public int Current { get { return _current; } }

        public OddEnumerator GetEnumerator()
        {
            return this;
        }
    }

    public OddEnumerator Odd(int max)
    {
        return new OddEnumerator(max);
    }

    void Start ()
    {
        Profiler.BeginSample("YieldTest");
        StartCoroutine(Test());
        Profiler.EndSample();

        // StartCoroutine(Test0());
        StartCoroutine(Test1());

        // 64byte
        {
            Profiler.BeginSample("IEnumerator size 0");
            StartCoroutine(Test0());
            Profiler.EndSample();
        }

        // 80byte
        {
            Profiler.BeginSample("IEnumerator size 1");
            StartCoroutine(Test2());
            Profiler.EndSample();
        }

        // 72byte
        {
            Profiler.BeginSample("IEnumerator size 2");
            StartCoroutine(Test3());
            Profiler.EndSample();
        }

        // 80byte
        {
            Profiler.BeginSample("IEnumerator size 3");
            StartCoroutine(Test4());
            Profiler.EndSample();
        }

        // 80byte
        {
            Profiler.BeginSample("IEnumerator size 4");
            StartCoroutine(Test5(array));
            Profiler.EndSample();
        }

        // 48byte
        {
            Profiler.BeginSample("IEnumerator foreach");
            foreach (var e in Even(1000000))
            {
            }
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("IEnumerator foreach 2");
            foreach (var e in Odd(1000000))
            {
            }
            Profiler.EndSample();
        }
    }
}
