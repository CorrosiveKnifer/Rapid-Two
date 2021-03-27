using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Rachael work
/// </summary>
public class BombExplosionScript : MonoBehaviour
{
    private bool isBombed = false;
    private float bombDamage = 0;
    private float timer = 1.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isBombed)
        {
            if (timer <= 0.0f)
            {
                Destroy(gameObject);
            }
            timer -= 0.5f;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        //checking when the bullet hits a enemy
        if (other.tag == "Enemy")
        {
            other.GetComponentInParent<EnemyScript>().DealDamageToEnemy(bombDamage);
            Debug.Log("Slow down");
            isBombed = true;
        }
    }
    public void SetDamage(float damageNum)
    {
        bombDamage = damageNum;
    }


}
