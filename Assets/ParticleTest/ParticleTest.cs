using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    [SerializeField]
    GameObject ps = null;

    void Start()
    {
        // trail使っていなくてもアクセスするとDefault-Materialがつく
        var pr = ps.GetComponent<ParticleSystemRenderer>();
        foreach (var m in pr.materials)
        {
            Debug.Log(m.name);
        }
    }
}
