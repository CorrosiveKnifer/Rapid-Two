using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    CharacterController m_CharacterController;
    Player m_Player;
    Camera m_Camera;
    private Animator animator;

    public GameObject materialLoc;
    public Material deathMaterial;

    public Vector3 RespawnPoint;

    [Header("Harvester Settings")]
    public float HarvestRadius = 2.5f;
    public float HarvestDelay = 0.5f;
    public float HarvestAmount = 1.0f;

    bool IsSelected = false;
    private float health;
    public float maxHealth = 100.0f;
    private bool IsDead = false;
    bool isMoving = false;
    public float m_fSpeed = 6;
    float fGravity = -9.81f;
    bool isGrounded = false;
    Vector3 velocity;

    public float bloodHold = 0.0f;
    public float maximumBlood = 100.0f;
    public float delay;

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
        
        if(!IsDead)
        {
            // Harvester movement
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            isMoving = (x != 0 || z != 0); // Is minion moving

            Vector3 move = (m_Player.transform.right * x + m_Player.transform.forward * z);
            m_CharacterController.Move((move * m_fSpeed + velocity) * Time.fixedDeltaTime);

            Vector3 lookAtTarget = transform.position + new Vector3(m_CharacterController.velocity.x, 0, m_CharacterController.velocity.z);
            transform.LookAt(lookAtTarget, Vector3.up);


            animator.SetFloat("MovementMod", Mathf.Clamp(m_fSpeed * 2.5f, 0.5f, 2.5f));
            animator.SetBool("IsMoving", isMoving);

            GameManager.instance.SetMinionBlood(bloodHold / maximumBlood);
        }
    }

    private void Update()
    {
        if (delay > 0)
            delay = Mathf.Clamp(delay - Time.deltaTime, 0, HarvestDelay);

        if (delay <= 0 && bloodHold < maximumBlood)
        {
            delay = HarvestDelay;
            GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("Blood");
            float expected = 0.0f;
            foreach (var foundObject in foundObjects)
            {
                if (Vector3.Distance(transform.position, foundObject.transform.position) < HarvestRadius)
                {
                    float amount = maximumBlood - bloodHold;

                    if (amount <= 0)
                        break;
                    if (expected > amount)
                        break;

                    if (amount < HarvestAmount)
                    {
                        foundObject.GetComponent<BloodScript>().Consume(this, amount);
                        expected += amount;
                        break;
                    }
                    else
                    {
                        foundObject.GetComponent<BloodScript>().Consume(this, HarvestAmount);
                        expected += HarvestAmount;
                    }
                }
            }
        }
    }

    public void Heal()
    {
        health = maxHealth;
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead)
            return;

        health -= damage;
        if (health <= 0)
        {
            GetComponent<VolumeAudioAgent>().PlaySoundEffect("HarvesterDeath");
            animator.SetTrigger("IsDead");
            IsDead = true;
            bloodHold = 0;
        }
        else
        {
            GetComponent<VolumeAudioAgent>().PlaySoundEffect($"HarvesterHurt{Mathf.FloorToInt(Random.Range(1, 5))}");
        }
    }

    protected void HandleShowDeathFinalFrame()
    {
        animator.SetTrigger("Reset");
        materialLoc.GetComponent<Renderer>().material.SetFloat("Alpha", 1.0f);
        IsDead = false;
        m_CharacterController.enabled = false;
        transform.position = RespawnPoint;
        m_CharacterController.enabled = true;
        health = maxHealth;
    }

    protected void PlayMovement()
    {
        //Do nothing
    }

    protected virtual GameObject FindClosestofTag(string tag, float range = -1)
    {
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(tag);
        float closestDistance = 100000;
        GameObject closestObject = null;
        foreach (var foundObject in foundObjects)
        {
            float distance = Vector3.Distance(transform.position, foundObject.transform.position);
            if (distance < closestDistance)
            {
                closestObject = foundObject;
                closestDistance = distance;
            }
        }

        if (range < 0)
        {
            return closestObject;
        }
        else if (closestDistance <= range)
        {
            return closestObject;
        }
        return null;
    }

    public void Death()
    {
        IsDead = true;
        HandleShowDeathFinalFrame();
    }

    //protected IEnumerator ShowDeath()
    //{
    //    float t = 0.0f;
    //    float dt = 0.05f;
    //    float dt2 = 0.01f;
    //    float alpha = 1.0f;
    //
    //    materialLoc.GetComponent<Renderer>().material = deathMaterial;
    //
    //    while (alpha > 0.0f)
    //    {
    //        alpha = Mathf.Lerp(1.0f, 0.0f, t);
    //        materialLoc.GetComponent<Renderer>().material.SetFloat("Alpha", alpha);
    //
    //        t += dt * Time.deltaTime;
    //        dt += dt2;
    //
    //        yield return new WaitForEndOfFrame();
    //    }
    //
    //    HandleShowDeathFinalFrame();
    //    yield return null;
    //}
}


