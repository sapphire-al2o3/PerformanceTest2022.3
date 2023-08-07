using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class MaterialPropertyBlockTest : MonoBehaviour
{
    void Start()
    {
        // 24byte
        Profiler.BeginSample("MaterialPropertyBlock");
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        Profiler.EndSample();

        // 0byte
        Profiler.BeginSample("MaterialPropertyBlock.SetColor");
        materialPropertyBlock.SetColor("_Color", Color.blue);
        Profiler.EndSample();

        var renderer = GetComponent<Renderer>();

        // 0byte
        Profiler.BeginSample("Renderer.SetPropertyBlock");
        renderer.SetPropertyBlock(materialPropertyBlock);
        Profiler.EndSample();

        // 0byte
        Profiler.BeginSample("Renderer.HasPropertyBlock");
        bool hasMPB = renderer.HasPropertyBlock();
        Profiler.EndSample();

        // 0byte
        Profiler.BeginSample("Renderer.GetPropertyBlock");
        renderer.GetPropertyBlock(materialPropertyBlock);
        Profiler.EndSample();
    }
}
