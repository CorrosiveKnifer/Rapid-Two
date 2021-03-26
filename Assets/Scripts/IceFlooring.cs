using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            //Slowing down the enemies
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
        }
    }
}
