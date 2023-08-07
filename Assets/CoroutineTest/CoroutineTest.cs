using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour {

    IEnumerator Test0()
    {
        while (true)
        {
            yield return null;
        }
    }

    IEnumerator Test1()
    {
        while (true)
        {
            yield return 0f;
        }
    }

	void Start()
    {
        for (int i = 0; i < 1000; i++)
        {
            StartCoroutine(Test0());
        }
	}
}
