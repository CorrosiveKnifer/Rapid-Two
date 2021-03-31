using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rachael work
/// </summary>
/// 
public class BombTowerScript : MonoBehaviour
{
    //main tower variables
    public GameObject ammo;
    public float bulletDamage = 1.0f;
    public float bulletSpeed = 1.0f;
    public float coolDown = 10.0f;
    private float towerCoolDown;
    public int cost = 15;

    //variables for the target enemy
    public GameObject[] enemies;
    public float towerRadius = 3.0f;
    public GameObject target;
    public GameObject indicator;
    bool isFirst = false;
    public Transform spawner;
    public GameObject m_Indicator;

    // Called before start
    private void Awake()
    {
        towerCoolDown = coolDown;

        //part of targetiing script
        target = null;

        //creating the indicator range
        m_Indicator = (GameObject)Instantiate(indicator, transform.position, transform.rotation);
        m_Indicator.transform.localScale *= (towerRadius * 2);
        m_Indicator.GetComponent<Renderer>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DetectEnemies();
        if (IsInRange())
        {
            TowerActivated();
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Reset");
        }
    }

    public void SetSelected(bool selected)
    {
        m_Indicator.GetComponent<Renderer>().enabled = selected;
    }

    public void TowerActivated()
    {
        GetComponent<Animator>().SetTrigger("IsShooting");
        GetComponent<Animator>().SetFloat("Delay", 1.0f / coolDown);

        ////a countdown on when to attack
        //if (towerCoolDown >= coolDown)
        //{
        //    //attack and reset counter
        //    towerCoolDown = 0;
        //    Fire();
        //}
        //towerCoolDown += 0.01f;
    }
    //function to spawn bullet
    void Fire()
    {
        GetComponent<VolumeAudioAgent>().PlaySoundEffect("FireTowerShoot");
        GameObject bulletClone = (GameObject)Instantiate(ammo, spawner.position, spawner.rotation);
        //bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        bulletClone.GetComponent<BombScript>().SetDamage(bulletDamage);
        bulletClone.GetComponent<BombScript>().SetSpeed(bulletSpeed);
        bulletClone.GetComponent<BombScript>().target = target;

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
            //Vector3 direction = target.transform.position - transform.position;

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Debug.DrawRay(transform.position, forward * towerRadius, Color.green);
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
    void OnDestroy()
    {
        // Will be called just prior to destruction of the gameobject to which this script is attached
        Destroy(m_Indicator);
    }
}
