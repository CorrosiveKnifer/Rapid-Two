using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public GameObject ammo;
    public float bulletSpeed = 1.0f;
    public float coolDown = 10.0f;
    private float towerCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        towerCoolDown = coolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (towerCoolDown >= coolDown)
        {
            towerCoolDown = 0;
            Fire();
        }
        towerCoolDown += 0.01f;
    }
    void Fire()
    {
        GameObject bulletClone = (GameObject)Instantiate(ammo, transform.position, transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
    }
}
