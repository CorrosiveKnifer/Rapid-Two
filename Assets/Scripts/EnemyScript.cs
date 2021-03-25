using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Michael Jordan
/// </summary>
public class EnemyScript : MonoBehaviour
{
    public TrailScript myPath;
    public enum AIState { IDLE, MOVING, HUNTING };

    public AIState currentState = AIState.IDLE;

    private NavMeshAgent agent;
    public int currentIndex = -1;

    public float health = 100.0f;

    public bool IsDead = false;

    private Animator controller;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponentInChildren<Animator>();

        if (myPath == null)
            Debug.LogError("Enemy Does not have a path.");

    }

    // Update is called once per frame
    void Update()
    {
        if(myPath != null && agent != null)
        {
            if (IsDead)
            {
                TransitionTo(AIState.IDLE);
                Destroy(agent);
                gameObject.AddComponent<Animator>();
                gameObject.layer = 13;
                controller.SetBool("IsDead", IsDead);
                Destroy(gameObject, 5.0f);
            }
            StateBasedUpdate();
        }
    }

    private void StateBasedUpdate()
    {
        switch (currentState)
        {
            case AIState.IDLE:
                {
                    if (currentIndex == -1)
                    {
                        agent.SetDestination(myPath.GetWaypointLocation(++currentIndex).position);
                        TransitionTo(AIState.MOVING);
                    }
                    break;
                }
            case AIState.MOVING:
                {
                    if(IsAgentAt(myPath.GetWaypointLocation(currentIndex).position, 2.0f))
                    {
                        agent.SetDestination(myPath.GetWaypointLocation(++currentIndex).position);
                    }

                    GameObject minion = GameObject.FindGameObjectWithTag("Minion");
                    if (Vector3.Distance(minion.transform.position, transform.position) < 5.0f)
                    {
                        agent.SetDestination(minion.transform.position);
                        TransitionTo(AIState.HUNTING);
                    } 
                    break;
                }
            case AIState.HUNTING:
                {
                    GameObject minion = GameObject.FindGameObjectWithTag("Minion");
                    if (Vector3.Distance(minion.transform.position, transform.position) < 5.0f)
                    {
                        agent.SetDestination(minion.transform.position);
                    }
                    else
                    {
                        agent.SetDestination(myPath.GetClosestWaypointTo(transform.position, out currentIndex).position);
                        TransitionTo(AIState.MOVING);
                    }
                    break;
                }
            default:
                break;
        }
    }

    private bool IsAgentAt(Vector3 destination, float offset = 1.0f)
    {
        Vector2 destinationPos = new Vector2(destination.x, destination.z);
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);

        return Vector2.Distance(currentPos, destinationPos) < offset;
    }

    private bool IsAgentFinished(float offset = 1.0f)
    {
        Vector2 destinationPos = new Vector2(agent.destination.x, agent.destination.z);
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);

        return Vector2.Distance(currentPos, destinationPos) < offset;
    }

    public float DealDamageToEnemy(float damage)
    {
        health -= damage;
        float overflow = (health <= 0) ? -1 * health : 0;

        if(health <= 0)
        {
            IsDead = true;
            return overflow;
        }
        return overflow;
    }

    private void TransitionTo(AIState newState)
    {
        if (currentState == newState)
            return;

        switch (newState)
        {
            case AIState.IDLE:
                break;
            case AIState.MOVING:
                agent.speed = 3.5f;
                break;
            case AIState.HUNTING:
                agent.speed = 4.0f;
                break;
            default:
                break;
        }

        currentState = newState;
    }
}
