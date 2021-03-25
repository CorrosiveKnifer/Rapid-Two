using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rachael work
/// </summary>
public class TowerScript : MonoBehaviour
{
    //main tower variables
    public GameObject ammo;
    public float bulletDamage = 1.0f; 
    public float bulletSpeed = 1.0f;
    public float coolDown = 10.0f;
    private float towerCoolDown;

    //variables for the target enemy
    public GameObject[] enemies;
    public float towerRadius = 3.0f;
    public GameObject target;
    public GameObject indicator;
    bool isFirst = false;

    // Start is called before the first frame update
    void Start()
    {
        
        towerCoolDown = coolDown;

        //part of targetiing script
        target = null;

        //creating the indicator range
        GameObject indicatorClone = (GameObject)Instantiate(indicator, transform.position, transform.rotation);
        indicatorClone.transform.localScale *= (towerRadius * 2);
    }

    // Update is called once per frame
    void Update()
    {
        DetectEnemies();
        if (IsInRange())
        {
            TowerActivated();
        }
    }

    public void TowerActivated()
    {
        //a countdown on when to attack
        if (towerCoolDown >= coolDown)
        {
            //attack and reset counter
            towerCoolDown = 0;
            Fire();
        }
        towerCoolDown += 0.01f;
    }
    //function to spawn bullet
    void Fire()
    {
        GameObject bulletClone = (GameObject)Instantiate(ammo, transform.position, transform.rotation);
        //bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        bulletClone.GetComponent<BulletScript>().SetDamage(bulletDamage);
        bulletClone.GetComponent<BulletScript>().target = target;
        
    }

    //function which activates the targeting of enemies for the tower created
    private void DetectEnemies()
    {
        //finding all enemies constantly
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //target = null;

        //if the enemy is close
        TargetCloset();

        //if enemy is first
        //TargetFirst();

        //MAKING the tower turn towards the player
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            Vector3 forward = transform.TransformDirection(direction);
            Debug.DrawRay(transform.position, forward, Color.green);
            //transform.LookAt(target.transform);
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
        target = null;
        float tempRadius = towerRadius;
        //checking each enemy
        foreach (GameObject enemy in enemies)
        {
            //calculate distance
            float enemydist = Vector3.Distance(enemy.transform.position, transform.position);
            //if its in tower range
            if (enemydist < tempRadius && !enemy.GetComponentInParent<EnemyScript>().IsDead)
            {
                //marking this as the closet enemy
                tempRadius = enemydist;
                target = enemy;

            }

        }
    }
    private void TargetLast()
    {
        //target = null;
        //float tempRadius = towerRadius;
        float farestEnemy = 0.0f;
        //checking each enemy
        foreach (GameObject enemy in enemies)
        {
            //calculate distance
            float enemydist = Vector3.Distance(enemy.transform.position, transform.position);
            //if its in tower range
            if (enemydist < towerRadius && !enemy.GetComponentInParent<EnemyScript>().IsDead)
            {
                //marking this as the furtherest enemy
                //marking the first enemy as the farest so far
                if (farestEnemy == 0)
                {
                    farestEnemy = enemydist;
                }
                //checking if there is another
                else
                {
                    if (farestEnemy <= enemydist)
                    {
                        target = enemy;
                    }
                }


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

    //i was attempting test a function where it picks the closest one and 
    //targets them until they leave, then picks the next closest thing
    private void TargetSpecial()
    {
        if (!isFirst)
        {
            float tempRadius = towerRadius;
            //checking each enemy
            foreach (GameObject enemy in enemies)
            {
                //calculate distance
                float enemydist = Vector3.Distance(enemy.transform.position, transform.position);
                //if its in tower range and the enemy isnt dead
                if (enemydist < tempRadius && !enemy.GetComponentInParent<EnemyScript>().IsDead)
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
            //if its iout of tower range
            if (enemydist >= towerRadius)
            {
                //marking this as the first enemy

                //target = null;
                isFirst = false;

            }
        }
    }

}
