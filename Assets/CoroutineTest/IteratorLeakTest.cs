using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IteratorLeakTest : MonoBehaviour
{
    IEnumerator Test()
    {
        for (int i = 0; i < 1; i++)
        {
            int[] array = new int[100000];
        }

        System.GC.Collect();

        // arrayはスコープ外になるので解放される

        while (true)
        {
            yield return null;
        }
    }

    IEnumerator TestLeak()
    {
        for (int i = 0; i < 2; i++)
        {
            int[] array = new int[100000];
            if (i == 1) break;
        }

        System.GC.Collect();

        // arrayはスコープ外になっているがbreakで抜けると参照を消されていないので解放されない

        while (true)
        {
            yield return null;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Test());
        StartCoroutine(TestLeak());
    }
}
