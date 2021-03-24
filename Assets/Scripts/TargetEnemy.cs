using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rachael work 
/// </summary>
public class TargetEnemy : MonoBehaviour
{
    public GameObject[] enemies;

    public float towerRadius = 3.0f;
    private Transform target;

    public GameObject indicator;
    // Start is called before the first frame update
    void Start()
    {
        target = null;

        //creating the indicator range
        GameObject indicatorClone = (GameObject)Instantiate(indicator, transform.position, transform.rotation);
        indicatorClone.transform.localScale *= (towerRadius*2);
      
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, forward* towerRadius, Color.green);

        //finding all enemies constantly
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float tempRadius = towerRadius;
        target = null;
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
    public bool IsInRange()
    {
        if (target != null)
        {
            return true;
        }
        return false;
    }


}
