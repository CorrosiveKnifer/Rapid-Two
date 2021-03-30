using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Rachael work
/// </summary>
public class BombScript : MonoBehaviour
{
    private float bulletdamage = 0;
    private float BulletHealth = 10;
    public GameObject target;
    public GameObject BombField;

    // Start is called before the first frame update
    void Start()
    {

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
        if (other.tag == "Enemy")
        {
            BulletHealth = other.GetComponentInParent<EnemyScript>().DealDamageToEnemy(0);
            //if it does hit
            if (BulletHealth == 0)
            {
                Effect();
                Destroy(gameObject);
            }
        }
    }

    void Effect()
    {
        GameObject BombEffect = (GameObject)Instantiate(BombField, target.transform.position, target.transform.rotation);
        BombEffect.GetComponent<BombExplosionScript>().SetDamage(bulletdamage);
    }

    //setting the bullets damage
    public void SetDamage(float damageNum)
    {
        bulletdamage = damageNum;
    }
}
