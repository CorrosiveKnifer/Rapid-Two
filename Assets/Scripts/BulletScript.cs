﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Rachaelwork
/// </summary>
public class BulletScript : MonoBehaviour
{
    private float bulletdamage = 0;
    private float BulletHealth = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //checking when the bullet hits a enemy
        if(other.tag == "Enemy")
        {
            BulletHealth = other.GetComponent<EnemyScript>().DealDamageToEnemy(bulletdamage);
            //if it does hit
            if(BulletHealth == 0)
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
