using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerAnimationlink : MonoBehaviour
{
    public void Fire()
    {
        GetComponentInParent<TowerScript>().Fire();
    }
}
