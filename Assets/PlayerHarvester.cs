using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    CharacterController m_CharacterController;
    Player m_Player;
    Camera m_Camera;
    private Animator animator;

    bool isMoving = false;
    public float m_fSpeed = 6;
    float fGravity = -9.81f;
    bool isGrounded = false;
    Vector3 velocity;

    public float bloodHold = 0.0f;
    public float maximumBlood = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Player = GameManager.instance.player;
        m_Camera = m_Player.GetComponentInChildren<Camera>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = m_CharacterController.isGrounded;
        if (isGrounded)
        {
            velocity.y = 0.0f;
        }
        else
        {
            velocity.y += fGravity * Time.fixedDeltaTime;
        }
        

        // Harvester movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        isMoving = (x != 0 || z != 0); // Is minion moving

        Vector3 move = (m_Player.transform.right * x + m_Player.transform.forward * z);
        Debug.Log(move);
        m_CharacterController.Move((move * m_fSpeed + velocity) * Time.fixedDeltaTime);

        Vector3 lookAtTarget = transform.position + new Vector3(m_CharacterController.velocity.x, 0, m_CharacterController.velocity.z);
        transform.LookAt(lookAtTarget, Vector3.up);


        animator.SetFloat("MovementMod", Mathf.Clamp(m_fSpeed * 2.5f, 0.5f, 2.5f));
        animator.SetBool("IsMoving", isMoving);

        GameManager.instance.SetMinionBlood(bloodHold / maximumBlood);
    }
}
