using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadTextureTest : MonoBehaviour
{
    [SerializeField]
    string url = null;

    [SerializeField]
    string url2 = null;

    void Start()
    {
        StartCoroutine(Donwload(url));
        StartCoroutine(DonwloadLeak(url2));
    }

    IEnumerator Donwload(string url)
    {
        using (var req = UnityWebRequestTexture.GetTexture(url, true))
        {
            yield return req.SendWebRequest();

            var tex = DownloadHandlerTexture.GetContent(req);

            Debug.Log(req.result);
        }

        Resources.UnloadUnusedAssets();

        // no leak texture
    }

    IEnumerator DonwloadLeak(string url)
    {
        using (var req = UnityWebRequestTexture.GetTexture(url, true))
        {
            yield return req.SendWebRequest();

            Debug.Log(req.result);
        }

        Resources.UnloadUnusedAssets();

		// leak texture
	}
}
