using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class ReflectionTest : MonoBehaviour
{
    class TestClass
    {
        int a;
    }

    struct TestStruct
    {
        int b;
    }

    T TestCreateInstance<T>()
    {
        return System.Activator.CreateInstance<T>();
    }

    T TestCreateInstance2<T>() where T : new()
    {
        return new T();
    }

    T TestCreateInstance3<T>() where T : struct
    {
        return default;
    }

    TestClass TestCreateInstance4()
    {
        return new TestClass();
    }

    // Start is called before the first frame update
    void Start()
    {
        List<string> l = new List<string>();

        // 0byte
        {
            Profiler.BeginSample("GetType");
            var t = l.GetType();
            Profiler.EndSample();
            Debug.Log(t);
        }

        // 0byte
        {
            Profiler.BeginSample("GetGenericTypeDefinition");
            var t = l.GetType();
            System.Type g = null;
            if (t.IsGenericType)
            {
                g = t.GetGenericTypeDefinition();
            }
            Profiler.EndSample();
            Debug.Log(g);
        }

        {
            var t = TestCreateInstance<TestClass>();
            var s = TestCreateInstance<TestStruct>();
        }

        // 20byte * 1000
        {
            Profiler.BeginSample("CreateInstance class");
            for (int i = 0; i < 1000; i++)
            {
                var t = TestCreateInstance<TestClass>();
            }
            Profiler.EndSample();
        }

        // 20byte * 1000
        {
            Profiler.BeginSample("CreateInstance struct");
            for (int i = 0; i < 1000; i++)
            {
                var t = TestCreateInstance<TestStruct>();
            }
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("generic new class");
            for (int i = 0; i < 1000; i++)
            {
                var t = TestCreateInstance2<TestClass>();
            }
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("generic new struct");
            for (int i = 0; i < 1000; i++)
            {
                var t = TestCreateInstance2<TestStruct>();
            }
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("new class");
            for (int i = 0; i < 1000; i++)
            {
                var t = TestCreateInstance4();
            }
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("generic default struct");
            for (int i = 0; i < 1000; i++)
            {
                var t = TestCreateInstance3<TestStruct>();
            }
            Profiler.EndSample();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
