using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Rachael work
/// </summary>
public class LaserTowerScript : MonoBehaviour
{
    //main tower variables
    public GameObject ammo;
    public float bulletDamage = 1.0f;
    public int cost = 25;

    //variables for the target enemy
    public GameObject[] enemies;
    public float towerRadius = 3.0f;
    public GameObject target;
    public GameObject indicator;
    bool isFirst = false;
    public LineRenderer ray;
    public GameObject m_Indicator;

    // Called before start
    private void Awake()
    {

        //part of targetiing script
        target = null;

        //creating the indicator range
        m_Indicator = (GameObject)Instantiate(indicator, transform.position, transform.rotation);
        m_Indicator.transform.localScale *= (towerRadius * 2);
        m_Indicator.GetComponent<Renderer>().enabled = false;
        ray.transform.localScale = new Vector3(1.0f, 1.0f, towerRadius);
        ray.GetComponent<Renderer>().enabled = false;
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
            GetComponent<VolumeAudioAgent>().PlaySoundEffect("LaserTowerShoot", true);
            GetComponent<Animator>().SetBool("IsShooting",  true);
        }
        else
        {
            GetComponent<VolumeAudioAgent>().StopAudio("LaserTowerShoot");
            GetComponent<Animator>().SetBool("IsShooting", false);
        }
    }

    public void SetSelected(bool selected)
    {
        m_Indicator.GetComponent<Renderer>().enabled = selected;
    }

    public void TowerActivated()
    {
        Fire();
    }
    //function to spawn bullet
    void Fire()
    {
        //since it knows where the target is, it will just shoot the constant beam
        target.GetComponentInParent<EnemyScript>().DealDamageToEnemy(bulletDamage * Time.deltaTime);
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

            Vector3 direction = target.transform.position - transform.position;
            float enemydist = Vector3.Distance(target.transform.position, transform.position);
            Debug.DrawRay(transform.position, direction.normalized * towerRadius, Color.blue);
            ray.transform.LookAt(target.transform);
            ray.transform.localScale = new Vector3(1.0f, 1.0f, enemydist +1.0f);
            
        }
    }

    public bool IsInRange()
    {
        if (target != null)
        {
            ray.GetComponent<Renderer>().enabled = true;
            return true;
        }
        ray.GetComponent<Renderer>().enabled = false;
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
