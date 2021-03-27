using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rachael work
/// </summary>
public class IceFlooring : MonoBehaviour
{
    private bool isIced = false;
    public float timer = 10.0f;
    public List<GameObject> allCollisions = new List<GameObject>();
    //public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isIced)
        {
            if (timer <= 0.0f)
            {
                for(int i =0; i < allCollisions.Count; i++)
                {
                    allCollisions[i].GetComponentInParent<EnemyScript>().SetMovementMod(1.0f);
                }
                Destroy(gameObject);
            }
            timer -= 0.1f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //checking when the bullet hits a enemy
        if (other.tag == "Enemy")
        {
            isIced = true;
            other.GetComponentInParent<EnemyScript>().SetMovementMod(0.5f);
            //Slowing down the enemies
            //Debug.Log(other.GetComponentInParent<EnemyScript>().MovementSpeedMod);
            Debug.Log("Slow down");
            allCollisions.Add(other.gameObject);
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
            allCollisions.Remove(other.gameObject);
            // Debug.Log(other.GetComponentInParent<EnemyScript>().MovementSpeedMod);
        }
    }
}
