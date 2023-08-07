using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class PrefabTest : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;

    [SerializeField]
    GameObject empty;

    void Start()
    {
        // 152byte
        {
            Profiler.BeginSample("prefab Instantiate");
            var go = GameObject.Instantiate(prefab);
            Profiler.EndSample();

            // 初回のtransformアクセスはキャッシュが作られる(40byte)
            Profiler.BeginSample("SetParent");
            go.transform.SetParent(transform);
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("prefab(empty) Instantiate");
            var go = GameObject.Instantiate(empty);
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("prefab(empty) Instantiate x100");
            for (int i = 0; i < 100; i++)
            {
                var tmp = GameObject.Instantiate(empty, transform);
                var x = tmp.transform.position.x;
            }
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("prefab Instantiate & SetParnet");
            var go = GameObject.Instantiate(prefab, transform);
            Profiler.EndSample();
        }


        {
            // 40byte
            Profiler.BeginSample("new GameObject");
            var go = new GameObject();
            Profiler.EndSample();

            // 0byte
            Profiler.BeginSample("set_name");
            go.name = "test";
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("new GameObject x100");
            for (int i = 0; i < 100; i++)
            {
                var go = new GameObject();
                go.transform.SetParent(transform);
            }
            Profiler.EndSample();
        }
    }
}
