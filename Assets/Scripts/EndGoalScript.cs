using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGoalScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            GameManager.instance.lives -= other.GetComponentInParent<EnemyScript>().livesCost;
            Destroy(other.transform.parent.gameObject);
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
