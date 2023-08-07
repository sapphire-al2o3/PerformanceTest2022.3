using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class EncodingTest : MonoBehaviour
{
    void Start()
    {
        // 19.6KB
        {
            Debug.Log(System.Text.Encoding.GetEncoding("UTF-8"));
            Profiler.BeginSample("GetEncoding(UTF-8)");
            string s = "hoge";
            for (int i = 0; i < 1000; i++)
            {
                System.Text.Encoding.GetEncoding("UTF-8").GetByteCount(s);
            }
            Profiler.EndSample();
        }

        {
            Debug.Log(System.Text.Encoding.UTF8);
            Profiler.BeginSample("UTF8");
            string s = "hoge";
            for (int i = 0; i < 1000; i++)
            {
                System.Text.Encoding.UTF8.GetByteCount(s);
            }
            Profiler.EndSample();
        }
    }
}
