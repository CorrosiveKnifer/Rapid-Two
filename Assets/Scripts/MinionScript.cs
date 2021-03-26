using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionScript : MonoBehaviour
{
    public Renderer selectedCircle;

    private NavMeshAgent agent;
    public float bloodCount = 0.0f;

    [Header("Minion Settings")]
    public float maximumBlood = 100.0f;
    public float minimumSpeed = 1.0f;
    public float maximumSpeed = 7.0f;

    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        selectedCircle.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //speed is inversly proportional to the blood count
        speed = Mathf.Clamp((1.0f - bloodCount/maximumBlood) * maximumSpeed, minimumSpeed, maximumSpeed);

        agent.speed = speed;

        foreach (var blood in GameObject.FindGameObjectsWithTag("Blood"))
        {
            Vector2 bloodPos = new Vector2(blood.transform.position.x, blood.transform.position.z);
            Vector2 myPos = new Vector2(transform.position.x, transform.position.z);
            if(Vector2.Distance(bloodPos, myPos) < 3.0f && !blood.GetComponent<BloodScript>().IsConsumed)
            {
                blood.GetComponent<BloodScript>().Consume(gameObject);
            }
        }

        if(IsAgentFinished())
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
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
