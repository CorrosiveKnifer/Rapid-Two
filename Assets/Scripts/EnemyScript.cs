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

    private NavMeshAgent agent;
    public int currentIndex = -1;

    public float health = 100.0f;

    public bool IsDead = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
                Destroy(agent);
                Destroy(gameObject, 5.0f);
            }

            if (currentIndex == -1)
            {
                agent.SetDestination(myPath.GetWaypointLocation(++currentIndex).position);
            }
            else if (IsAgentFinished(2.0f))
            {
                if (myPath.waypointCount - 1 == currentIndex)
                    return;

                agent.SetDestination(myPath.GetWaypointLocation(++currentIndex).position);
            }
            else
            {
                GameObject minion = GameObject.FindGameObjectWithTag("Minion");
                if (Vector3.Distance(minion.transform.position, transform.position) < 5.0f)
                {
                    agent.SetDestination(minion.transform.position);
                }
                else
                {
                    agent.SetDestination(myPath.GetWaypointLocation(currentIndex).position);
                }
            }
        }

        GetComponent<Animator>().SetBool("IsDead", IsDead);
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
}
