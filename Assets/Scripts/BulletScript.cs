using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Rachaelwork
/// </summary>
public class BulletScript : MonoBehaviour
{
    private float bulletdamage = 0;
    private float BulletHealth = 10;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        if (target != null)
        {
            transform.LookAt(target.transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;

            GetComponent<Rigidbody>().velocity = direction.normalized * 8.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //checking when the bullet hits a enemy
        if(other.tag == "Enemy")
        {
            BulletHealth = other.GetComponentInParent<EnemyScript>().DealDamageToEnemy(bulletdamage);
            //if it does hit
            if(BulletHealth == 0 || other.GetComponentInParent<EnemyScript>().IsDead)
            {
                Destroy(gameObject);
            }
        }
    }
    //setting the bullets damage
    public void SetDamage(float damageNum)
    {
        bulletdamage = damageNum;
    }
}
