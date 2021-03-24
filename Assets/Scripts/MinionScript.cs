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
        
    }

    public void SetTargetLocation(Vector3 positon)
    {
        agent.destination = positon;
    }

    public void SetSelected(bool selected)
    {
        selectedCircle.enabled = selected;
    }
}
