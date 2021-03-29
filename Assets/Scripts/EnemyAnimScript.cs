using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimScript : MonoBehaviour
{
    public void Attack()
    {
        GetComponentInParent<EnemyScript>().Attack();
    }
}
