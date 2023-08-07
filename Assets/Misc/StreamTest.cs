using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System.IO;

public class StreamTest : MonoBehaviour
{
    unsafe void UnsafeWrite(BinaryWriter bw, float value)
    {
        uint temp = *(uint*)&value;
        bw.Write(temp);
    }

    void Start()
    {
        byte[] buffer = null;

        using (var ms = new MemoryStream(1024))
        using (var bw = new BinaryWriter(ms))
        {
            // 36byte
            using (new ProfilerScope("float"))
            {
                float f = 1.0f;
                bw.Write(f);
            }

            Debug.Log(ms.Length);

            // 0byte
            using (new ProfilerScope("int"))
            {
                int i = 1;
                bw.Write(i);
            }

            Debug.Log(ms.Length);

            // 0byte
            using (new ProfilerScope("float unsafe"))
            {
                float f = 1.234f;
                UnsafeWrite(bw, f);
            }

            Debug.Log(ms.Length);

            // 文字列書き込み用のバッファ(256byte)が確保される
            // 440byte
            using (new ProfilerScope("string"))
            {
                string str = "";
                bw.Write(str);
            }

            Debug.Log(ms.Length);

            buffer = ms.GetBuffer();
        }

        using (var ms = new MemoryStream(buffer))
        using (var br = new BinaryReader(ms))
        {
            // 0byte
            using (new ProfilerScope("read float"))
            {
                float f = br.ReadSingle();
                Debug.Assert(f == 1.0f);
            }

            // 0byte
            using (new ProfilerScope("read int"))
            {
                int i = br.ReadInt32();
                Debug.Assert(i == 1);
            }

            {
                float f = br.ReadSingle();
                Debug.Assert(f == 1.234f);
            }
        }
    }
}
