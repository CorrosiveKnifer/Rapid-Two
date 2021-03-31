using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAnimLink : MonoBehaviour
{
    public void Attack()
    {
        GetComponentInParent<DemonScript>().DealDamage();
    }
    public void PlayDeathSound()
    {
        GetComponentInParent<DemonScript>().DeathSound();
    }
    public void Death()
    {
        GetComponentInParent<DemonScript>().Death();
    }
    public void Spawn()
    {
        GetComponentInParent<DemonScript>().PlaySpawnAudio();
    }
}
