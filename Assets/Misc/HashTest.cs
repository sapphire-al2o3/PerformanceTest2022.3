using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using UnityEngine.Profiling;
using System.Text;
using System;

public class HashTest : MonoBehaviour
{
    string GetHashString(byte[] hash)
    {
        var sb = new StringBuilder();
        foreach (var h in hash)
        {
            sb.Append(h.ToString("x2"));
        }
        return sb.ToString();
    }

	string GetHashString(ReadOnlySpan<byte> hash)
	{
		var sb = new StringBuilder();
		foreach (var h in hash)
		{
			sb.Append(h.ToString("x2"));
		}
		return sb.ToString();
	}

    void Start()
    {
        byte[] data = { 0, 1, 2, 3, 4, 5, 6, 7 };

        // 192B
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                Profiler.BeginSample("ComputeHash");
                var hash = md5.ComputeHash(data);
                Profiler.EndSample();

                Debug.Log(GetHashString(hash));
            }
        }

        // 240B
        {
            using (var md5 = IncrementalHash.CreateHash(HashAlgorithmName.MD5))
            {
                Profiler.BeginSample("IncrementalHash");
                md5.AppendData(data);
                var hash = md5.GetHashAndReset();
                Profiler.EndSample();

                Debug.Log(GetHashString(hash));
            }
        }

        // 240B
        // 配列からSpanにコピーされているだけ？
        {
            using (var md5 = IncrementalHash.CreateHash(HashAlgorithmName.MD5))
            {
                Profiler.BeginSample("IncrementalHash2");
                md5.AppendData(data);
                Span<byte> hash = stackalloc byte[32];
                md5.TryGetHashAndReset(hash, out var length);
                Profiler.EndSample();

                Debug.Log(GetHashString(hash.Slice(0, length)));
            }
        }

        // 228B
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                Profiler.BeginSample("TransformBlock");
                md5.TransformBlock(data, 0, 4, null, 0);
                md5.TransformBlock(data, 4, 4, null, 0);
                md5.TransformFinalBlock(data, 0, 0);
                var hash = md5.Hash;
                Profiler.EndSample();

                Debug.Log(GetHashString(hash));
            }
        }

        {
            byte[] data2 = new byte[1024 * 1024];
            for (int i = 0; i < data2.Length; i++)
            {
                data2[i] = 0;
            }
            using (var md5 = new MD5CryptoServiceProvider())
            {
                // 0.5MB
                int blockSize = 512 * 1024;
                Profiler.BeginSample("TransformBlock 2");
                int i = 0;
                for (i = 0; i < data2.Length - blockSize; i += blockSize)
                {
                    md5.TransformBlock(data2, i, blockSize, null, 0);
                }
                md5.TransformFinalBlock(data2, i, blockSize);
                var hash = md5.Hash;
                Profiler.EndSample();
                Debug.Log(GetHashString(hash));

                // 1MB
                md5.Initialize();
                Profiler.BeginSample("TransformBlock 3");
                md5.TransformFinalBlock(data2, 0, data2.Length);
                hash = md5.Hash;
                Profiler.EndSample();
                Debug.Log(GetHashString(hash));

                // 192B
                md5.Initialize();
                Profiler.BeginSample("TransformBlock 4");
                i = 0;
                for (i = 0; i < data2.Length; i += blockSize)
                {
                    md5.TransformBlock(data2, i, blockSize, null, 0);
                }
                md5.TransformFinalBlock(data2, 0, 0);
                hash = md5.Hash;
                Profiler.EndSample();
                Debug.Log(GetHashString(hash));
            }
        }

        // 320B
        {
            Profiler.BeginSample("MD5CryptoServiceProvider");
            var md5 = new MD5CryptoServiceProvider();
            Profiler.EndSample();

            Debug.Log(md5.GetType());
        }

        // 1.5KB
        {
            Profiler.BeginSample("MD5.Create");
            var md5 = MD5.Create();
            Profiler.EndSample();

            Debug.Log(md5.GetType());
        }
    }
}
