using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StacktraceTest : MonoBehaviour
{
    void Start()
    {
        // GC Alloc‚ÌCall stacks•\¦‚ª‚·‚×‚Ä“¯‚¶s‚É‚È‚é
        int[] a = new int[1];




        int[] b = new int[2];




        int[] c = new int[3];
    }
}
