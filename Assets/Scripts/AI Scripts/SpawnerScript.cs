using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public Transform SpawnLoc;

    public TrailScript myPath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool SpawnGameObject(GameObject prefab)
    {
         EnemyScript enemy = GameObject.Instantiate(prefab, SpawnLoc.position, SpawnLoc.rotation).GetComponent<EnemyScript>();
         enemy.myPath = myPath;
         return true;
    }
}
