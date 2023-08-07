using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildAssetBundle : Editor
{
    [MenuItem("Editor/Build AssetBundles")]
    static void Build()
    {
        var option = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DisableWriteTypeTree;
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", option, BuildTarget.Android);
    }
}
