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

        // array�̓X�R�[�v�O�ɂȂ�̂ŉ�������

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

        // array�̓X�R�[�v�O�ɂȂ��Ă��邪break�Ŕ�����ƎQ�Ƃ�������Ă��Ȃ��̂ŉ������Ȃ�

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
