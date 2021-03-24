using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rachael work on editing
/// </summary>
public class TargetEnemy : MonoBehaviour
{
    public GameObject[] enemies;

    public float towerRadius = 3.0f;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = null;
    }

    // Update is called once per frame
    void Update()
    {
        //finding all enemies constantly
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float tempRadius = towerRadius;

        //checking each enemy
        foreach (GameObject enemy in enemies)
        {
            //calculate distance
            float enemydist = Vector3.Distance(enemy.transform.position, transform.position);
            //if its in tower range
            if (enemydist < tempRadius)
            {
                //marking this as the closet enemy
                tempRadius = enemydist;
                target = enemy.transform;

            }

        }
        if(target != null)
        {
            transform.LookAt(target);
        }

       

    }


}
