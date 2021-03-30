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
    public float AttackRange = 2.5f;
    public float BloodAmount = 5.0f;
    public float Damage = 5.0f;
    public float baseSpeed = 3.5f;
    public int livesCost = 1;

    public bool IsDead = false;
    public GameObject BloodPrefab;
    private Animator controller;
    private GameObject target;
    private float MovementSpeedMod = 1.0f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = baseSpeed;
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
                    blood.transform.LookAt(hit.point + hit.normal, Vector3.up);
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

                    target = FindClosestMinion();
                    if (target != null)
                    {
                        agent.SetDestination(target.transform.position);
                        TransitionTo(AIState.HUNTING);
                    } 
                    break;
                }
            case AIState.HUNTING:
                {
                    target = FindClosestMinion();
                    if (target == null)
                    {
                        agent.SetDestination(myPath.GetWaypointLocation(currentIndex).position);
                        TransitionTo(AIState.MOVING);
                        return;
                    }

                    float distance = Vector3.Distance(target.transform.position, transform.position);
                    if (distance < AttackRange)
                    {
                        TransitionTo(AIState.ATTACKING);
                    }
                    if (distance < HuntRange)
                    {
                        agent.SetDestination(target.transform.position);
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
                    target = FindClosestMinion();
                    if(target == null)
                    {
                        agent.SetDestination(myPath.GetWaypointLocation(currentIndex).position);
                        TransitionTo(AIState.MOVING);
                        return;
                    }

                    float distance = Vector3.Distance(target.transform.position, transform.position);
                    if (distance < AttackRange)
                    {
                        agent.isStopped = true;
                        TransitionTo(AIState.ATTACKING);
                    }
                    else if(distance < HuntRange)
                    {
                        agent.isStopped = false;
                        agent.SetDestination(target.transform.position);
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

    private GameObject FindClosestMinion()
    {
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("Minion");
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

        if (HuntRange < 0)
        {
            return closestObject;
        }
        else if (closestDistance <= HuntRange)
        {
            return closestObject;
        }
        return null;
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

    public void Attack()
    {
        if (target != null)
        {
            target.GetComponent<MinionScript>().TakeDamage(Damage);
        }
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
                agent.isStopped = false;
                agent.speed = baseSpeed * MovementSpeedMod;
                break;
            case AIState.HUNTING:
                agent.isStopped = false;
                agent.speed = (baseSpeed + 0.5f) * MovementSpeedMod;
                break;
            case AIState.ATTACKING:
                agent.speed = (baseSpeed - 1.0f) * MovementSpeedMod;
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

    public void StartFreeze()
    {
        if (MovementSpeedMod != 0)
        {
            StartCoroutine(Freeze());
        }
    }
    IEnumerator Freeze()
    {
        Color oldColor = GetComponentInChildren<MeshRenderer>().material.color;
        // Freeze.
        GetComponentInChildren<MeshRenderer>().material.color = new Color(0.5f, 0.7f, 0.8f);
        SetMovementMod(0.0f);

        // Freeze duration.
        yield return new WaitForSeconds(5.0f);

        // Unfreeze.
        GetComponentInChildren<MeshRenderer>().material.color = oldColor;
        SetMovementMod(1.0f);
    }
}
