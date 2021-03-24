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
        if(currentIndex == -1)
        {
            agent.SetDestination(myPath.GetWaypointLocation(++currentIndex).position);
        }
        else if(IsAgentFinished(2.0f))
        {
            if (myPath.waypointCount - 1 == currentIndex)
                return;

            agent.SetDestination(myPath.GetWaypointLocation(++currentIndex).position);
        }
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
            Destroy(gameObject, 0.05f);
            return overflow;
        }
        return overflow;
    }
}
