using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// William de Beer
/// </summary>

public class CameraControl : MonoBehaviour
{
    [Header("Values")]
    public float fCameraSpeed = 0.3f;

    [Header("Attachements")]
    public GameObject m_Camera;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        transform.position += (transform.right * x + transform.forward * z) * fCameraSpeed;
    }
}
