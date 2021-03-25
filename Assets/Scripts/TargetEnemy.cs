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
    public GameObject target;

    public GameObject indicator;

    bool isFirst = false;

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
        //target = null;

        //if the enemy is close
        //TargetCloset();

        //if enemy is first
        TargetFirst();

        if (target != null)
        {
            transform.LookAt(target.transform);
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
    private void TargetCloset()
    {
        //target = null;
        float tempRadius = towerRadius;
        //checking each enemy
        foreach (GameObject enemy in enemies)
        {
            //calculate distance
            float enemydist = Vector3.Distance(enemy.transform.position, transform.position);
            //if its in tower range
            if (enemydist < tempRadius && !enemy.GetComponent<EnemyScript>().IsDead)
            {
                //marking this as the closet enemy
                tempRadius = enemydist;
                target = enemy;

            }

        }
    }
    private void TargetFirst()
    {

        target = null;
        //checking each enemy
        foreach (GameObject enemy in enemies)
            {
                //calculate distance
                float enemydist = Vector3.Distance(enemy.transform.position, transform.position);
                //if its in tower range
                if (enemydist < towerRadius && !enemy.GetComponentInParent<EnemyScript>().IsDead)
                {
                    //marking this as the closet enemy
                    
                    target = enemy;
                    break;

                }

            }
        
        
    }
    private void TargetTesting()
    {
        if (!isFirst)
        {
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
                    target = enemy;
                    isFirst = true;

                }

            }
        }
        else
        {
            //calculate distance
            float enemydist = Vector3.Distance(target.transform.position, transform.position);
            //if its in tower range
            if (enemydist >= towerRadius)
            {
                //marking this as the first enemy

                target = null;
                isFirst = false;

            }
        }
    }


}
