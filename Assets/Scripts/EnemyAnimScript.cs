using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
public class EnemyAnimScript : MonoBehaviour
{
    public void Attack()
    {
        GetComponentInParent<EnemyScript>().Attack();
    }
}
