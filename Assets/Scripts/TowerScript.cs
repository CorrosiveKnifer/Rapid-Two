using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rachael work
/// </summary>
public class TowerScript : MonoBehaviour
{
    
    public GameObject ammo;
    public float bulletDamage = 1.0f; 
    public float bulletSpeed = 1.0f;
    public float coolDown = 10.0f;
    private float towerCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        
        towerCoolDown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<TargetEnemy>().IsInRange())
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
        bulletClone.GetComponent<BulletScript>().target = GetComponent<TargetEnemy>().target;
    }


}
