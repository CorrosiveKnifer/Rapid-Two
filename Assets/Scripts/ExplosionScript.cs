using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private ParticleSystem system;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VolumeAudioAgent>().PlaySoundEffect("Explosion");
        system = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.
        if (system.time >= 0.99f)
            Destroy(gameObject);
    }
}
