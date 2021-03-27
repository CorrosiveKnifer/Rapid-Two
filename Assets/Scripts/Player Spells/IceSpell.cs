﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// William
/// </summary>
public class IceSpell : MonoBehaviour
{
    float m_fSpeed = 0.3f;
    public GameObject IceField;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += -transform.up * m_fSpeed;
    }

    private void Explode()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Instantiate(IceField, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Explode();
        }
    }
}

