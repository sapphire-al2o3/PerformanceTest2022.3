using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Profiling;

public class PathTest : MonoBehaviour
{
    void Start()
    {
        string path = "Assets/PathTest.cs";
        // 3.7KB
        {
            Profiler.BeginSample("GetDirectoryName");
            string dir = null;
            for (int i = 0; i < 100; i++)
            {
                dir = Path.GetDirectoryName(path);
            }
            Profiler.EndSample();

            Debug.Log(dir);
        }

        string path2 = "PathTest.cs";
        // 0B
        {
            Profiler.BeginSample("GetDirectoryName empty");
            string dir = null;
            for (int i = 0; i < 100; i++)
            {
                dir = Path.GetDirectoryName(path2);
            }
            Profiler.EndSample();

            Debug.Log(dir);
        }
    }
}
