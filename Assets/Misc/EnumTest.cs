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
        // IL‚ð‚Ý‚é‚Æbox‚ª‚æ‚Ñ‚¾‚³‚ê‚Ä‚¢‚é‚¯‚Ç0byte
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
    }
}
