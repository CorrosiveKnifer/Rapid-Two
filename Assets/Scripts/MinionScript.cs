using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionScript : MonoBehaviour
{
    public Renderer selectedCircle;

    protected NavMeshAgent agent;
    public float bloodCount = 0.0f;

    [Header("Minion Settings")]
    public float maximumBlood = 100.0f;
    public float minimumSpeed = 1.0f;
    public float maximumSpeed = 7.0f;
    public float speedMod = 1.0f;

    protected float speed;
    protected bool IsSelected = false;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        selectedCircle.enabled = IsSelected;
        //speed is inversly proportional to the blood count
        speed = Mathf.Clamp((1.0f - bloodCount/maximumBlood) * maximumSpeed, minimumSpeed, maximumSpeed);

        agent.speed = speed * speedMod;
    }

    public void SetTargetLocation(Vector3 positon)
    {
        agent.isStopped = false;
        agent.destination = positon;
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;
    }

    protected bool IsAgentFinished(float offset = 1.0f)
    {
        Vector2 destinationPos = new Vector2(agent.destination.x, agent.destination.z);
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);

        return Vector2.Distance(currentPos, destinationPos) < offset;
    }
}
