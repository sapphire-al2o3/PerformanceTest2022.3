using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StacktraceTest : MonoBehaviour
{
    void Start()
    {
        // GC Alloc��Call stacks�\�������ׂē����s�ɂȂ�
        int[] a = new int[1];




        int[] b = new int[2];




        int[] c = new int[3];
    }
}
