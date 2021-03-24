using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public Transform SpawnLoc;

    public TrailScript myPath;

    public float delay = 0.5f;
    private float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
            timer -= Time.deltaTime;
    }

    public bool SpawnGameObject(GameObject prefab)
    {
        if(timer <= 0.0f)
        {
            EnemyScript enemy = GameObject.Instantiate(prefab, SpawnLoc.position, SpawnLoc.rotation).GetComponent<EnemyScript>();
            enemy.myPath = myPath;
            timer = delay;
            return true;
        }
        return false;
    }
}
