using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
public class BloodScript : MonoBehaviour
{
    public float bloodCount { get; set; }
    public GameObject bloodSpherePrefab;
    public bool IsConsumed = false;

    private GameObject myConsumer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsConsumed)
        {
            if(bloodCount > 0)
            {
                GameObject blood = GameObject.Instantiate(bloodSpherePrefab, transform);
                blood.GetComponent<BloodProjScript>().target = myConsumer;
                bloodCount -= 1.0f;
            }
            else
            {
                Destroy(gameObject, 3.0f);
            }
        }
    }

    public void Consume(GameObject consumer)
    {
        IsConsumed = true;
        myConsumer = consumer;
        GetComponentInChildren<Animator>().SetTrigger("Consume");
    }
}
