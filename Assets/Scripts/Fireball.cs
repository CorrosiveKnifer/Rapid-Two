using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// William
/// </summary>
public class Fireball : MonoBehaviour
{
    float m_fSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += -transform.up * m_fSpeed;
    }

    private void Explode()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");


        for (int i = 0; i < enemies.Length; i++)
        {
            if (Vector3.Distance(transform.position, enemies[i].transform.position) < 5.0f)
            {
                enemies[i].GetComponent<EnemyScript>().DealDamageToEnemy(80);
            }
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Explode();
        }
    }
}
