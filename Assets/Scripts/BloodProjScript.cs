using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
public class BloodProjScript : MonoBehaviour
{
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime);

            if(Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                target.GetComponent<MinionScript>().bloodCount += 1.0f;
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
