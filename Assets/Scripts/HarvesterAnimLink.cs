using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
public class HarvesterAnimLink : MonoBehaviour
{
    public void Death()
    {
        GetComponentInParent<HarvesterScript>().Death();
    }
}
