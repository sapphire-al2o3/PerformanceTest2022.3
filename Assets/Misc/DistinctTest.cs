using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DistinctTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<int> list = new List<int>(300);

        for (int i = 0; i < 100; i++)
        {
            list.Add(i);
            list.Add(i);
            list.Add(i);
        }

        int sum = 0;
        using (var ps = new ProfilerScope("Linq"))
        {
            foreach (var e in list.Distinct())
            {
                sum += e;
            }
        }

        Debug.Log(sum);

        sum = 0;

        using (new ProfilerScope("Sort"))
        {
            list.Sort();
            var l = new List<int>();
            int t = list[0];
            l.Add(t);
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] != t)
                {
                    t = list[i];
                    sum += t;
                }
            }
        }

        

        Debug.Log(sum);

        //Debug.Log(t);
        //for (var i = 1; i < list.Count; i++)
        //{
        //    if (t != list[i])
        //    {
        //        t = list[i];
        //        Debug.Log(t);
        //    }
        //}
        {
            list.Sort();
            int t = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] == t)
                {
                    list.RemoveAt(i);
                }
                else
                {
                    t = list[i];
                }
            }
        }
    }
}
