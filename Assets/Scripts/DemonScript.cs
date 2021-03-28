using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonScript : MinionScript
{
    [Header("Demon Settings")]
    public float maxHealth = 100.0f;
    public float DetectRadius = 12.5f;
    public float AttackRadius = 2.5f;
    public float Damage;

    private enum AIState { DETECT, SELECTED, ATTACK };
    private AIState currentState;
    private GameObject myHuntTarget;
    private bool IsDead = false;
    private float health;

    // Start is called before the first frame update
    protected override void Start()
    {
        health = maxHealth;
        base.Start();
    }

    protected override void Update()
    {
        GetComponentInChildren<Animator>().SetBool("IsMoving", !IsAgentFinished());
        StateBasedUpdate();
        base.Update();
    }

    private void StateBasedUpdate()
    {
        if (IsDead)
        {
            return;
        }

        if (IsSelected)
        {
            TransitionTo(AIState.SELECTED);
        }
        else if (currentState == AIState.SELECTED)
        {
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
            case AIState.ATTACK:
                {
                    AttackUpdate();
                    break;
                }
            default:
                break;
        }
    }

    /*--------------------
     * Detect Mode
     * -------------------
     * The soul intent of this mode is to constantly look out for the nearest enemy to target.
     * Once a blood is within range, which is returned by the FindClosestofTag function, it will transition 
     * to the HARVEST Mode.
     */
    private void DetectUpdate()
    {
        //myHuntTarget = FindClosestofTag("End");
        //
        //SetTargetLocation(myHuntTarget.transform.position);
        //
        //if (IsAgentFinished())
        //{
        //    TransitionTo(AIState.DETECT);
        //}
    }

    /*--------------------
     * Selected Mode
     * -------------------
     * The soul intent of this state is to allow the player resume full control of this
     * minion. Where the minion will attack any enemies that are near-by.
     */
    private void SelectedUpdate()
    {
        if (IsAgentFinished())
        {
            agent.isStopped = true;

            if (myHuntTarget == null)
            {
                //Find closest target.
                myHuntTarget = FindClosestofTag("Enemy", DetectRadius);
            }
            else
            {
                float distance = Vector3.Distance(transform.position, myHuntTarget.transform.position);
                if (distance > DetectRadius)
                {
                    //Distance is too far, forget about this one!
                    myHuntTarget = null;
                }
                else if (distance > AttackRadius)
                {
                    //Do nothing, it isn't close enough
                }
                else
                {
                    ////Start harvesting
                    //if (delay <= 0)
                    //{
                    //    delay = HarvestDelay;
                    //    myHuntTarget?.GetComponent<BloodScript>().Consume(this, HarvestAmount);
                    //}
                }
            }
        }
    }

    /*--------------------
     * Deposit Mode
     * -------------------
     * The soul intent of this state is to attack and push back all enemies in an area.
     */
    private void AttackUpdate()
    {
        myHuntTarget = FindClosestofTag("End");
        //
        SetTargetLocation(myHuntTarget.transform.position);
        //
        if (IsAgentFinished())
        {
            GetComponentInChildren<Animator>().SetBool("IsAttacking", true);
            //TransitionTo(AIState.DETECT);
        }
    }

    private void TransitionTo(AIState newState)
    {
        if (currentState == newState)
            return;

        switch (newState)
        {
            case AIState.DETECT:
                GetComponentInChildren<Animator>().SetBool("IsAttacking", false);
                myHuntTarget = null;
                break;
            case AIState.SELECTED:
                GetComponentInChildren<Animator>().SetBool("IsAttacking", false);
                myHuntTarget = null;
                break;
            case AIState.ATTACK:
                break;
            default:
                break;
        }

        currentState = newState;
    }

    public void DealDamage()
    {

    }
}
