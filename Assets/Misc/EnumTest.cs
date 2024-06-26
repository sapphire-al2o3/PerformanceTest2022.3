using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class EnumTest : MonoBehaviour
{
    [System.Flags]
    enum Flags
    {
        A,
        B,
        C
    }

    void Start()
    {
        // ILをみるとboxがよびだされているけど0byte
        // 0byte
        {
            Profiler.BeginSample("Enum.HasFlags");
            Flags flag = Flags.A | Flags.B;
            bool ret = flag.HasFlag(Flags.B);
            for (int i = 0; i < 100; i++)
            {
                ret |= flag.HasFlag(Flags.C);
            }
            Profiler.EndSample();

            Debug.Log(ret);
        }

        // 20byte
        {
            Profiler.BeginSample("Enum.ToObject");
            int i = 0;
            Flags flag = (Flags)System.Enum.ToObject(typeof(Flags), i);
            Profiler.EndSample();

            Debug.Log(flag);
        }

        // 188byte
        using (new ProfilerScope("Enum.GetValues"))
        {
            var values = System.Enum.GetValues(typeof(Flags));
        }

        // 92byte
        {
            var values = System.Enum.GetValues(typeof(Flags));
            using (new ProfilerScope("foreach"))
            {
                foreach (var t in values)
                {
                }
            }
        }

        // 56byte
        using (new ProfilerScope("Enum.GetNames"))
        {
            var names = System.Enum.GetNames(typeof(Flags));
        }
        
    }
}
