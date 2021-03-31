using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
public class HarvesterScript : MinionScript
{
    public float bloodHold;

    [Header("Harvester Settings")]
    public float maxHealth = 100.0f;
    public float maximumBlood = 100.0f;
    public float DetectRadius = 12.5f;
    public float HarvestRadius = 2.5f;
    public float HarvestDelay = 0.5f;
    public float HarvestAmount = 1.0f;
    public Vector3 RespawnPoint;

    private enum AIState { DETECT, SELECTED, HARVEST, DEPOSIT};
    private AIState currentState;
    private GameObject myHuntTarget;
    private float delay;
    private float health;
    private Animator animator;

    protected override void Start()
    {
        animator = GetComponentInChildren<Animator>();
        transform.position = RespawnPoint;
        health = maxHealth;
        base.Start();
    }

    protected override void Update()
    {
        if (delay > 0)
            delay = Mathf.Clamp(delay - Time.deltaTime, 0, HarvestDelay);

        float speedRange = maximumSpeed - minimumSpeed;
        speed = Mathf.Clamp((1.0f - bloodHold / maximumBlood) * speedRange + minimumSpeed, minimumSpeed, maximumSpeed);
        animator.SetFloat("MovementMod", Mathf.Clamp((speed / maximumSpeed) * 2.5f, 0.5f, 2.5f));
        animator.SetBool("IsMoving", !agent.isStopped);

        GameManager.instance.SetMinionBlood(bloodHold / maximumBlood);

        StateBasedUpdate();

        base.Update();
    }

    private void StateBasedUpdate()
    {
        if(IsSelected)
        {
            TransitionTo(AIState.SELECTED);
        }
        else if (currentState == AIState.SELECTED && IsAgentFinished())
        {
            agent.isStopped = true;
            TransitionTo(AIState.DETECT);
        }

        switch (currentState)
        {
            case AIState.DETECT:
                {
                    DetectUpdate();
                    break;
                }
            case AIState.SELECTED:
                {
                    SelectedUpdate();
                    break;
                }
            case AIState.HARVEST:
                {
                    HarvestUpdate();
                    break;
                }
            case AIState.DEPOSIT:
                {
                    DepositUpdate();
                    break;
                }
            default:
                break;
        }
    }

    /*--------------------
     * Detect Mode
     * -------------------
     * The soul intent of this mode is to constantly look out for the nearest blood to harvest.
     * Once a blood is within range, which is returned by the FindClosestofTag function, it will transition 
     * to the HARVEST Mode.
     */
    private void DetectUpdate()
    {
        GameObject blood = myHuntTarget = FindClosestofTag("Blood", DetectRadius);
        if (blood != null)
        {
            myHuntTarget = blood;
            TransitionTo(AIState.HARVEST);
        }
    }

    /*--------------------
     * Selected Mode
     * -------------------
     * The soul intent of this state is to allow the player resume full control of this
     * minion. Where the minion will only harvest from a blood pool if the player has moved them
     * to it. Only transitioning out of this state iff the player deselects this minion.
     */
    private void SelectedUpdate()
    {
        if (IsAgentFinished())
        {
            agent.isStopped = true;
            myHuntTarget = FindClosestofTag("Blood", DetectRadius);
            animator.SetBool("IsConsuming", false);

            if (myHuntTarget != null)
            {
                float distance = Vector3.Distance(transform.position, myHuntTarget.transform.position);
                if (distance > DetectRadius)
                {
                    //Distance is too far, forget about this one!
                    myHuntTarget = null;
                    animator.SetBool("IsConsuming", false);
                }
                else if (distance > HarvestRadius)
                {
                    //Do nothing, it isn't close enough
                    animator.SetBool("IsConsuming", false);
                }
                else
                {
                    agent.isStopped = true;
                    animator.SetBool("IsConsuming", true);
                    //Start harvesting
                    if (delay <= 0)
                    {
                        delay = HarvestDelay;
                        myHuntTarget?.GetComponent<BloodScript>().Consume(this, HarvestAmount);
                    }
                }
            }
        }
    }

    /*--------------------
     * Harvest Mode
     * -------------------
     * The soul intent of this state is to continuiously harvest from a pool of blood until it has
     * despawned. Once either the target has despawned or if the harvester is out of detect range of
     * the targeted pool it will transition back to the DETECT mode.
     */
    private void HarvestUpdate()
    {
        if (bloodHold >= maximumBlood)
        {
            TransitionTo(AIState.DEPOSIT);
            animator.SetBool("IsConsuming", false);
            return;
        }

        if (myHuntTarget == null)
        {
            TransitionTo(AIState.DETECT);
            animator.SetBool("IsConsuming", false);
            return;
        }
        
        float distance = Vector3.Distance(transform.position, myHuntTarget.transform.position);

        if (distance > DetectRadius) //Transition back to detect
        {
            TransitionTo(AIState.DETECT);
            animator.SetBool("IsConsuming", false);
        }
        else if (distance > HarvestRadius)
        {
            SetTargetLocation(myHuntTarget.transform.position);
            animator.SetBool("IsConsuming", false);
        }
        else
        {
            agent.isStopped = true;
            animator.SetBool("IsConsuming", true);
            if (delay <= 0 && bloodHold < maximumBlood)
            {
                delay = HarvestDelay;
                GameObject[] bloods = GameObject.FindGameObjectsWithTag("Blood");
                float expected = 0.0f;
                foreach (var blood in bloods)
                {
                    if(Vector3.Distance(transform.position, blood.transform.position) < HarvestRadius)
                    {
                        float amount = maximumBlood - bloodHold;
                        
                        if (amount <= 0)
                            break;
                        if (expected > amount)
                            break;

                        if(amount < HarvestAmount)
                        {
                            blood.GetComponent<BloodScript>().Consume(this, amount);
                            expected += amount;
                            break;
                        }
                        else
                        {
                            blood.GetComponent<BloodScript>().Consume(this, HarvestAmount);
                            expected += HarvestAmount;
                        }
                        
                    }
                }
                //float amount = Mathf.Min(HarvestAmount, maximumBlood - bloodHold);
               // myHuntTarget?.GetComponent<BloodScript>().Consume(this, amount);
            }
        }
    }

    /*--------------------
     * Deposit Mode
     * -------------------
     * The soul intent of this state is to deliver the blood to the end goal whenever the blood
     * hold is full.
     */
    private void DepositUpdate()
    {
        myHuntTarget = FindClosestofTag("End");

        SetTargetLocation(myHuntTarget.transform.position);
        //animator.SetBool("IsMoving", true);

        if (IsAgentFinished())
        {
            //animator.SetBool("IsMoving", false);
            TransitionTo(AIState.DETECT);
        }
    }

    private void TransitionTo(AIState newState)
    {
        if (currentState == newState)
            return;

        switch (newState)
        {
            case AIState.DETECT:
                myHuntTarget = null;
                break;
            case AIState.SELECTED:
                break;
            case AIState.HARVEST:
                break;
            case AIState.DEPOSIT:
                myHuntTarget = null;
                break;
            default:
                break;
        }

        currentState = newState;
    }

    public override void TakeDamage(float damage)
    {
        if (IsDead)
            return;

        health -= damage;
        if (health <= 0)
        {
            animator.SetTrigger("IsDead");
            IsDead = true;
            bloodHold = 0;
        }
    }

    protected override void HandleShowDeathFinalFrame()
    {
        animator.SetTrigger("Reset");
        materialLoc.GetComponent<Renderer>().material.SetFloat("Alpha", 1.0f);
        IsDead = false;
        agent.isStopped = true;
        agent.enabled = false;
        transform.position = RespawnPoint;
        agent.enabled = true;
        health = maxHealth;
    }

    protected override void PlayMovement()
    {
        //Do nothing
    }
}
