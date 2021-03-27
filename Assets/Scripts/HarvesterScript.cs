using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HarvesterScript : MinionScript
{
    public float bloodHold;

    [Header("Harvester Settings")]
    public float health;
    public float maximumBlood = 100.0f;
    public float DetectRadius = 12.5f;
    public float HarvestRadius = 2.5f;
    public float HarvestDelay = 0.5f;
    public float HarvestAmount = 1.0f;

    private enum AIState { DETECT, SELECTED, HARVEST, DEPOSIT};
    private AIState currentState;
    private GameObject myHuntTarget;
    private float delay;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (delay > 0)
            delay = Mathf.Clamp(delay - Time.deltaTime, 0, HarvestDelay);

        float speedRange = maximumSpeed - minimumSpeed;
        speed = Mathf.Clamp((1.0f - bloodHold / maximumBlood) * speedRange + minimumSpeed, minimumSpeed, maximumSpeed);

        StateBasedUpdate();
        base.Update();
    }

    private void StateBasedUpdate()
    {
        if(IsSelected)
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

            if (myHuntTarget == null)
            {
                //Find closest target.
                myHuntTarget = FindClosestofTag("Blood", DetectRadius);
            }
            else
            {
                float distance = Vector3.Distance(transform.position, myHuntTarget.transform.position);
                if (distance > DetectRadius)
                {
                    //Distance is too far, forget about this one!
                    myHuntTarget = null;
                }
                else if (distance > HarvestRadius)
                {
                    //Do nothing, it isn't close enough
                }
                else
                {
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
            return;
        }

        if (myHuntTarget == null)
        {
            TransitionTo(AIState.DETECT);
            return;
        }

        float distance = Vector3.Distance(transform.position, myHuntTarget.transform.position);

        if (distance > DetectRadius) //Transition back to detect
        {
            TransitionTo(AIState.DETECT);
        }
        else if (distance > HarvestRadius)
        {
            SetTargetLocation(myHuntTarget.transform.position);
        }
        else
        {
            agent.isStopped = true;
            if (delay <= 0)
            {
                delay = HarvestDelay;
                float amount = Mathf.Min(HarvestAmount, maximumBlood - bloodHold);
                myHuntTarget?.GetComponent<BloodScript>().Consume(this, amount);
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

        if (IsAgentFinished())
        {
            TransitionTo(AIState.DETECT);
        }
    }

    private GameObject FindClosestofTag(string tag, float range = -1)
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

        if(range < 0)
        {
            return closestObject;
        }
        else if (closestDistance <= range)
        {
            return closestObject;
        }
        return null;
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
}
