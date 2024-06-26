﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System.IO;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;

public class FileTest : MonoBehaviour
{
	[SerializeField]
	string filePath = null;

	[SerializeField]
	string dirPath = null;

    private void Start()
    {
#if UNITY_EDITOR
        Run();
#endif
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Run();
        }
    }


    void Run()
    {
        string file = Application.dataPath + "/" + filePath;
        string dir = Application.dataPath + "/" + dirPath;

        Debug.Log(Application.dataPath);

        // 0B
        // IL2CPP 0.5KB
        // IL2CPP 2回目以降 0B
        {
            Profiler.BeginSample("Exists File");
            bool exists = File.Exists(file);
            Profiler.EndSample();

            Debug.Log(exists);
        }

        // 0.9KB
        // IL2CPP 1.5KB
        // IL2CPP 2回目以降 0.8KB
        // パスの長さに依存する
        {
            Profiler.BeginSample("Exists Directory");
            bool exists = Directory.Exists(dir);
            Profiler.EndSample();

            Debug.Log(exists);
        }

        // 1.1KB
        {
            Profiler.BeginSample("Exists FileInfo");
            FileInfo fi = new FileInfo(file);
            bool exists = fi.Exists;
            Profiler.EndSample();

            Debug.Log(exists);
        }

        // 1.1KB
        {
            Profiler.BeginSample("Size FileInfo");
            FileInfo fi = new FileInfo(file);
            long size = fi.Length;
            Profiler.EndSample();

            Debug.Log($"file size: {size}");
        }

        // 3.9KB
        {
            Profiler.BeginSample("Size FileStream");
            long size = 0;
            using (var fs = File.OpenRead(file))
            {
                size = fs.Length;
            }
            Profiler.EndSample();

            Debug.Log($"file size: {size}");
        }

        // 20.2KB
        {
            Profiler.BeginSample("GetFiles");
            string[] files = Directory.GetFiles(dir);
            Profiler.EndSample();

            foreach (var e in files)
            {
                Debug.Log(e);
            }
        }

        // 20.0KB
        {
            Profiler.BeginSample("GetDirectories *");
            string[] directories = Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly);
            Profiler.EndSample();

            foreach (var e in directories)
            {
                Debug.Log(e);
            }
        }

        // 19.9KB
        {
            Profiler.BeginSample("EnumerateDirectories");
            string[] directories = Directory.EnumerateDirectories(dir, "*", SearchOption.TopDirectoryOnly).ToArray();
            Profiler.EndSample();

            foreach (var e in directories)
            {
                Debug.Log(e);
            }
        }

        // 21.4KB
        {
            Profiler.BeginSample("GetFileSystemEntries");
            string[] files = Directory.GetFileSystemEntries(dir);
            Profiler.EndSample();

            foreach (var e in files)
            {
                Debug.Log(e);
            }
        }

        unsafe
        {
            Profiler.BeginSample("AsyncReadManager.GetFileInfo");
            FileInfoResult fileInfo;
            var handle = AsyncReadManager.GetFileInfo(file, &fileInfo);
            Profiler.EndSample();

            Debug.Log(handle.Status);
            handle.Dispose();
            Debug.Log(fileInfo.FileState);
            Debug.Log(fileInfo.FileSize);
        }
    }
}
