using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rachael work
/// </summary>
public class IceFlooring : MonoBehaviour
{
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
        if (other.tag == "Enemy")
        {
            other.GetComponentInParent<EnemyScript>().SetMovementMod(0.5f);
            //Slowing down the enemies
            //Debug.Log(other.GetComponentInParent<EnemyScript>().MovementSpeedMod);
            Debug.Log("Slow down");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //checking when the bullet hits a enemy
        if (other.tag == "Enemy")
        {
            //putting the enemies speed back
            Debug.Log("Speed Back");
            other.GetComponentInParent<EnemyScript>().SetMovementMod(1.0f);
           // Debug.Log(other.GetComponentInParent<EnemyScript>().MovementSpeedMod);
        }
    }
}
