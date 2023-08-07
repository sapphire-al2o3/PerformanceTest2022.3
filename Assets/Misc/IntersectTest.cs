using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System.Linq;

public class IntersectTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<int> a3 = new List<int>();
        List<int> a5 = new List<int>();

        for (int i = 1; i <= 1000; i++)
        {
            if (i % 3 == 0)
            {
                a3.Add(i);
            }
            if (i % 5 == 0)
            {
                a5.Add(i);
            }
        }


        Profiler.BeginSample("intersect linq");
        List<int> a15 = a3.Intersect(a5).ToList();
        Profiler.EndSample();

        foreach (var e in a15)
        {
            Debug.Log(e);
        }

        Profiler.BeginSample("intersect");
		int count = a3.Count > a5.Count ? a5.Count : a3.Count;
        List<int> b15 = new List<int>(count);
        //foreach (var e in a3)
        //{
        //    if (a5.Contains(e))
        //    {
        //        b15.Add(e);
        //    }
        //}
		int start = 0;
		for (int i = 0; i < a3.Count; i++)
		{
			for (int j = start; j < a5.Count; j++)
			{
				if (a3[i] == a5[j])
				{
					start = j + 1;
					b15.Add(a5[j]);
					break;
				}
			}
		}
        Profiler.EndSample();

        foreach (var e in b15)
        {
            Debug.Log(e);
        }

		List<int> c5 = new List<int>(a5);
        Profiler.BeginSample("intersect list");
        int s = a5.Count;
        for (int i = 0; i < a3.Count; i++)
        {
            if (a5.Contains(a3[i]))
            {
                a5.Add(a3[i]);
            }
        }
        a5.RemoveRange(0, s);
        Profiler.EndSample();

		Debug.Log(a5.Count);


    }
}
