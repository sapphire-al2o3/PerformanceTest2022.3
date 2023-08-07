using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class CastTest : MonoBehaviour
{
    enum EnumNum
    {
        Num0,
        Num1,
        Num2,
        Num3
    }

    [Flags]
    enum EnumFlags
    {
        Num0,
        Num1,
        Num2,
        Num3
    }

	public int Hoge(int x) => x * 2;

    public int Hoge2(int x)
    {
        return x * 2;
    }

    public System.Func<int, int> Hoge3 = x => x * 2;

    public void ForEach(int[] array, System.Func<int, int> func)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = func(array[i]);
        }
    }

    void Start()
    {
        EnumNum n = EnumNum.Num3;



        // 0byte
        Profiler.BeginSample("cast () enum -> int");
        int i = (int)n;
        Profiler.EndSample();

        // 40byte
        Profiler.BeginSample("cast Convert.ToInt32 enum -> int");
        i = System.Convert.ToInt32(n);
        Profiler.EndSample();

        // 20byte
        Profiler.BeginSample("cast Enum.ToObject int -> enum");
        n = (EnumNum)System.Enum.ToObject(typeof(EnumNum), i);
        Profiler.EndSample();

        // 0byte
        Profiler.BeginSample("cast () int -> enum");
        n = (EnumNum)i;
        Profiler.EndSample();

        // 20byte
        Profiler.BeginSample("cast () int -> object");
        object o = i;
        Profiler.EndSample();

        // 0byte
        Profiler.BeginSample("cast () object -> int");
        i = (int)o;
        Profiler.EndSample();

        // 24byte
        {
            Profiler.BeginSample("cast () long -> object");
            long l = 1000;
            object lo = l;
            Profiler.EndSample();
            
            Debug.Log(lo);
        }

        int[] array = new int[100];
        for (int k = 0; k < array.Length; k++)
        {
            array[k] = k;
        }

        Profiler.BeginSample("lamda 4");
        for (int k = 0; k < 100; k++)
        {
            ForEach(array, x => x * 2);
        }
        Profiler.EndSample();

        // 112byte
        {
            Profiler.BeginSample("delegate");
            Func<int, int> func = Hoge;
            Profiler.EndSample();
        }

        // 48byte
        {
            Profiler.BeginSample("nullable");
            int? x = null;
            if (x == null)
                x = 10;
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("nullable 2");
            int? x = null;
            if (x == null)
                x = 10;
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("enum flags");
            EnumFlags flags = EnumFlags.Num1 | EnumFlags.Num2;
            flags.HasFlag(EnumFlags.Num1);
            Profiler.EndSample();
        }
    }
}
