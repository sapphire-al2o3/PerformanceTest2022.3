using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;

public class UITest : MonoBehaviour
{
    [SerializeField]
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        {
            Profiler.BeginSample("set_text");
            text.text = "aaaaaa";
            Profiler.EndSample();
        }
    }
}
