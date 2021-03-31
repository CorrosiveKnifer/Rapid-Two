using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// William
/// </summary>
public class Fireball : MonoBehaviour
{
    float m_fSpeed = 20.0f;
    public float m_damage = 30.0f;
    public GameObject ExplositionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += -transform.up * m_fSpeed * Time.fixedDeltaTime;
    }

    private void Explode()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++)
        {
            if (Vector3.Distance(transform.position, enemies[i].transform.position) < 5.0f)
            {
                enemies[i].GetComponentInParent<EnemyScript>().DealDamageToEnemy(m_damage);
            }
        }
        GameObject.Instantiate(ExplositionPrefab, transform.position, Quaternion.identity);
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
