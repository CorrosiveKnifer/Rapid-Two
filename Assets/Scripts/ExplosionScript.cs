using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private ParticleSystem system;
    // Start is called before the first frame update
    void Start()
    {
        system = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (system.time >= 1.0f)
            Destroy(gameObject);
    }
}
