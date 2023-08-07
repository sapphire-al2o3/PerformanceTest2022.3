using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System.Runtime.InteropServices;

public class AlignmentTest : MonoBehaviour
{
    class A
    {
        string a;
        byte b;
        string c;
        byte d;
    }

    // 40byte
    class B
    {
        string a;
        string b;
        byte c;
        byte d;
    }

    // 40byte
    class B1
    {
        string a;
        string b;
        byte c;
    }

    // 16byte
    class Empty
    {
    }

    // 24byte
    class A1
    {
        string a;
    }

    // 17byte
    class A2
    {
        byte a;
    }

    // 18byte
    class A3
    {
        byte a;
        byte b;
    }

    // 32byte
    class A4
    {
        byte a;
        string b;
    }

    // 32byte
    class A5
    {
        string b;
        int a;
    }

    // 32byte
    class A6
    {
        string b;
        string a;
    }

    // 28byte
    class C1
    {
        byte a;
        int b;
        byte c;
    }

    [StructLayout(LayoutKind.Auto)]
    class C1_A
    {
        byte a;
        int b;
        byte c;
    }

    // 24byte
    class C2
    {
        int b;
        byte a;
        byte c;
    }

    // 22byte
    [StructLayout(LayoutKind.Auto, Pack = 1)]
    class C3
    {
        byte a;
        int b;
        byte c;
    }

    // 22byte
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class C4
    {
        byte a;
        int b;
        byte c;
    }

    class C5
    {
        byte a;
        short b;
        byte c;
    }

    // 48byte
    class C6
    {
        byte a;
        int b;
        byte c;
        int d;
        byte e;
        int f;
        byte g;
        int h;
    }

    // 36byte
    class C7
    {
        int b;
        int d;
        int f;
        int h;
        byte a;
        byte c;
        byte e;
        byte g;
    }


    struct S1
    {
        byte a;
        int b;
        byte c;
    }

    // エディタで実行すると参照型はクラスの先頭に配置される？

    // 40byte
    // Android IL2CPPだと48byte
    class C8
    {
        int a;
        string b;
        int c;
        string d;
    }

    // 48byte
    class C8_2
    {
        int a;
        long b;
        int c;
        string d;
    }

    // 40byte
    class C8_3
    {
        int a;
        string b;
        int c;
        long d;
    }

    // 48byte
    class C9
    {
        int a;
        string b;
        long c;
        string d;
    }

    // 48byte
    [StructLayout(LayoutKind.Sequential)]
    class C10
    {
        int a;
        string b;
        int c;
        string d;
    }

    [StructLayout(LayoutKind.Auto)]
    class C10_A
    {
        int a;
        string b;
        int c;
        string d;
    }

    // 48byte
    class C11
    {
        int a;
        long b;
        int c;
        long d;
    }

    // 40byte
    class C12
    {
        int a;
        int b;
        long c;
        long d;
    }

    // 40byte
    class C13
    {
        int a;
        C1 b;
        int c;
        C1 d;
    }


    void Start()
    {
        Profiler.BeginSample("A");
        var a = new A6();
        Profiler.EndSample();

        Profiler.BeginSample("B");
        B b = new B();
        Profiler.EndSample();

        Profiler.BeginSample("S1");
        S1 s1 = new S1();
        Profiler.EndSample();

        Profiler.BeginSample("Empty");
        Empty e0 = new Empty();
        Profiler.EndSample();

        Profiler.BeginSample("C1");
        C1 c1 = new C1();
        Profiler.EndSample();

        Profiler.BeginSample("C1_A");
        var c1_a = new C1_A();
        Profiler.EndSample();

        Profiler.BeginSample("C2");
        C2 c2 = new C2();
        Profiler.EndSample();

        Profiler.BeginSample("C3");
        C3 c3 = new C3();
        Profiler.EndSample();

        Profiler.BeginSample("C4");
        C4 c4 = new C4();
        Profiler.EndSample();

        Profiler.BeginSample("C5");
        C5 c5 = new C5();
        Profiler.EndSample();

        Profiler.BeginSample("C6");
        C6 c6 = new C6();
        Profiler.EndSample();

        Profiler.BeginSample("C7");
        C7 c7 = new C7();
        Profiler.EndSample();

        Profiler.BeginSample("C8");
        var c8 = new C8();
        Profiler.EndSample();

        Profiler.BeginSample("C8_2");
        var c8_2 = new C8_2();
        Profiler.EndSample();

        Profiler.BeginSample("C8_3");
        var c8_3 = new C8_3();
        Profiler.EndSample();

        Profiler.BeginSample("C9");
        var c9 = new C9();
        Profiler.EndSample();

        Profiler.BeginSample("C10");
        var c10 = new C10();
        Profiler.EndSample();

        Profiler.BeginSample("C11");
        var c11 = new C11();
        Profiler.EndSample();

        Profiler.BeginSample("C12");
        var c12 = new C12();
        Profiler.EndSample();

        Profiler.BeginSample("C13");
        var c13 = new C13();
        Profiler.EndSample();
    }
}
