using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System;

public class SpanTest : MonoBehaviour
{
    void Start()
    {
        // ���\�b�h�I�����ɃX�^�b�N���������(�X�R�[�v�ƈႤ)
        // IL2CPP�ł�alloca���Ăяo�����
        // 0byte
        {
            Profiler.BeginSample("stackalloc");
            Span<int> span = stackalloc int[10];
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = i;
            }
            Profiler.EndSample();
        }
    }
}
