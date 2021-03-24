using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public TrailScript myPath;

    private NavMeshAgent agent;
    public int currentIndex = -1;

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
        else if(IsAgentFinished())
        {
            if (myPath.waypointCount - 1 == currentIndex)
                return;

            agent.SetDestination(myPath.GetWaypointLocation(++currentIndex).position);
        }
    }

    private bool IsAgentFinished()
    {
        Vector2 destinationPos = new Vector2(agent.destination.x, agent.destination.z);
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);

        return currentPos == destinationPos;
    }
}
