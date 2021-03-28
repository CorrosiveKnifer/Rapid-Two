using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonScript : MinionScript
{
    [Header("Demon Settings")]
    public float maxHealth = 100.0f;
    public float DetectRadius = 12.5f;
    public float Damage;

    private enum AIState { DETECT, SELECTED, HARVEST, DEPOSIT };
    private AIState currentState;
    private GameObject myHuntTarget;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
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
