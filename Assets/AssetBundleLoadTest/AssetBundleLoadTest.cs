using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AssetBundleLoadTest : MonoBehaviour
{
    [SerializeField]
    string path = null;

    void LoadSync()
    {
        var ab = AssetBundle.LoadFromFile(path);
        var go = ab.LoadAsset<GameObject>(Path.GetFileNameWithoutExtension(path));
        ab.Unload(false);
        Instantiate(go);
    }

    IEnumerator LoadAsync()
    {
        var ab = AssetBundle.LoadFromFileAsync(path);
        yield return ab;
        var req = ab.assetBundle.LoadAssetAsync<GameObject>(Path.GetFileNameWithoutExtension(path));
        yield return req;
        ab.assetBundle.Unload(false);
        Instantiate(req.asset);
    }

    void OnGUI()
    {
        if (GUILayout.Button("Load Sync", GUILayout.Width(200), GUILayout.Height(100)))
        {
            LoadSync();
        }

        if (GUILayout.Button("Load Async", GUILayout.Width(200), GUILayout.Height(100)))
        {
            StartCoroutine(LoadAsync());
        }
    }
}
