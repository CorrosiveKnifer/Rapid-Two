using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionScript : MonoBehaviour
{
    public Renderer selectedCircle;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        selectedCircle.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsAgentFinished())
        {
            agent.isStopped = true;
        }
    }

    public void SetTargetLocation(Vector3 positon)
    {
        agent.destination = positon;
    }

    public void SetSelected(bool selected)
    {
        selectedCircle.enabled = selected;
    }
    private bool IsAgentFinished(float offset = 1.0f)
    {
        Vector2 destinationPos = new Vector2(agent.destination.x, agent.destination.z);
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);

        return Vector2.Distance(currentPos, destinationPos) < offset;
    }
}
