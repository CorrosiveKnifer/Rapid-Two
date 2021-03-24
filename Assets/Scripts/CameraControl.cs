using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// William de Beer
/// </summary>

public class CameraControl : MonoBehaviour
{
    [Header("Values")]
    public float fCameraMoveSpeed = 0.3f;
    public float fCameraZoomSpeed = 0.3f;

    [Header("Attachements")]
    public Camera m_Camera;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        transform.position += (transform.right * x + transform.forward * z) * fCameraMoveSpeed;

        m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize - Input.mouseScrollDelta.y * fCameraZoomSpeed, 1, 15);
        
    }
}
