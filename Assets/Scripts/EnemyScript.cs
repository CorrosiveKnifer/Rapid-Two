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
    public enum AIState { IDLE, MOVING, HUNTING, ATTACKING };

    public AIState currentState = AIState.IDLE;

    private NavMeshAgent agent;
    public int currentIndex = -1;

    [Header("Enemy Settings")]
    public float health = 100.0f;
    public float HuntRange = 5.0f;
    public float AttackRange = 1.5f;
    public float BloodAmount = 5.0f;

    public bool IsDead = false;
    public GameObject BloodPrefab;
    private Animator controller;
    private float MovementSpeedMod = 1.0f;

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
                RaycastHit hit;
                if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, 3.0f))
                {
                    GameObject blood = GameObject.Instantiate(BloodPrefab, hit.point + new Vector3(0.0f, 0.01f, 0.0f), Quaternion.identity);
                    blood.GetComponent<BloodScript>().bloodCount = BloodAmount;
                }

                TransitionTo(AIState.IDLE);
                Destroy(agent);
                controller.SetBool("IsDead", IsDead);
                Destroy(gameObject, 2.0f);
            }
            StateBasedUpdate();
        }

        if (controller != null)
            controller.SetBool("IsAttacking", currentState == AIState.ATTACKING && !IsDead);
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
                    float distance = Vector3.Distance(minion.transform.position, transform.position);
                    if (distance < AttackRange)
                    {
                        TransitionTo(AIState.ATTACKING);
                    }
                    if (distance < HuntRange)
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
            case AIState.ATTACKING:
                {
                    GameObject minion = GameObject.FindGameObjectWithTag("Minion");
                    float distance = Vector3.Distance(minion.transform.position, transform.position);
                    if (distance < AttackRange)
                    {
                        agent.isStopped = true;
                        TransitionTo(AIState.ATTACKING);
                    }
                    else if(distance < HuntRange)
                    {
                        agent.isStopped = false;
                        agent.SetDestination(minion.transform.position);
                        TransitionTo(AIState.HUNTING);
                    }
                    else
                    {
                        agent.isStopped = false;
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
                agent.speed = 3.5f * MovementSpeedMod;
                break;
            case AIState.HUNTING:
                agent.speed = 4.0f * MovementSpeedMod;
                break;
            case AIState.ATTACKING:
                agent.speed = 1.5f * MovementSpeedMod;
                break;
            default:
                break;
        }

        currentState = newState;
    }

    public void SetMovementMod(float mod)
    {
        MovementSpeedMod = mod;

        switch (currentState)
        {
            case AIState.IDLE:
                break;
            case AIState.MOVING:
                agent.speed = 3.5f * MovementSpeedMod;
                break;
            case AIState.HUNTING:
                agent.speed = 4.0f * MovementSpeedMod;
                break;
            case AIState.ATTACKING:
                agent.speed = 1.5f * MovementSpeedMod;
                break;
            default:
                break;
        }
    }
}
