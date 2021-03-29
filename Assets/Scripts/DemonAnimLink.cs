using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAnimLink : MonoBehaviour
{
    public void Attack()
    {
        GetComponentInParent<DemonScript>().DealDamage();
    }

    public void Spawn()
    {

    }
}
