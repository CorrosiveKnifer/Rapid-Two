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
            PlayerHarvester minion = other.GetComponent<PlayerHarvester>();
            if (minion != null)
            {
                GameManager.instance.blood += minion.bloodHold;
                minion.bloodHold = 0;
            }
        }
    }

    public void StartSummonOfDemon()
    {
        Animator[] animators = GetComponentsInChildren<Animator>();
        foreach (var anim in animators)
        {
            anim.SetTrigger("Summon");
        }
    }
}
