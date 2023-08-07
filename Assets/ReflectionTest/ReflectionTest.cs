using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class ReflectionTest : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
