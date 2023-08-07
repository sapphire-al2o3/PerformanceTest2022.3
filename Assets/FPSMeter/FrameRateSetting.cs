using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateSetting : MonoBehaviour
{
    [SerializeField]
    int frameRate = 60;

    void Start()
    {
        Application.targetFrameRate = frameRate;
        QualitySettings.vSyncCount = 0;
    }
}
