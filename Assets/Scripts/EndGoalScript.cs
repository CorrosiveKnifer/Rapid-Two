using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGoalScript : MonoBehaviour
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
        if(other.tag == "Enemy")
        {
            GameManager.instance.lives -= 1;
            Destroy(other.gameObject);
        }
        if (other.tag == "Minion")
        {
            HarvesterScript minion = other.GetComponent<HarvesterScript>();
            if (minion != null)
            {
                GameManager.instance.blood += minion.bloodHold;
                minion.bloodHold = 0;
            }
        }
    }
}
