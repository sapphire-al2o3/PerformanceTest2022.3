using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTest : MonoBehaviour
{
    void Start()
    {
        var renderer = GetComponent<Renderer>();

        var mats = renderer.sharedMaterials;
        Debug.Log(mats[0].GetHashCode());

        var mat = renderer.sharedMaterial;
        Debug.Log(mat.GetHashCode());

        mats = renderer.sharedMaterials;
        Debug.Log(mats[0].GetHashCode());

        // マテリアルをインスタンス化したあとはsharedMaterialも同じマテリアルを返す
        mats = renderer.materials;
        Debug.Log(mats[0].GetHashCode());

        mats = renderer.sharedMaterials;
        Debug.Log(mats[0].GetHashCode());

        mat = renderer.material;
        Debug.Log(mat.GetHashCode());
    }
}
