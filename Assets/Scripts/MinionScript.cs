using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class MinionScript : MonoBehaviour
{
    public Renderer selectedCircle;

    protected NavMeshAgent agent;

    [Header("Minion Settings")]
    public float minimumSpeed = 1.0f;
    public float maximumSpeed = 7.0f;
    public float speedMod = 1.0f;

    public GameObject materialLoc;

    protected float speed;
    protected bool IsDead = false;
    protected bool IsSelected = false;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        speed = (maximumSpeed + minimumSpeed) / 2.0f;
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        selectedCircle.enabled = IsSelected;
        //speed is inversly proportional to the blood count

        agent.speed = speed * speedMod;
    }

    public void SetTargetLocation(Vector3 positon)
    {
        agent.isStopped = false;
        agent.destination = positon;
        PlayMovement();
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;
    }

    public abstract void TakeDamage(float damage);
    protected abstract void HandleShowDeathFinalFrame();
    protected abstract void PlayMovement();

    protected bool IsAgentFinished(float offset = 1.0f)
    {
        Vector2 destinationPos = new Vector2(agent.destination.x, agent.destination.z);
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);

        return Vector2.Distance(currentPos, destinationPos) < offset;
    }

    protected virtual GameObject FindClosestofTag(string tag, float range = -1)
    {
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(tag);
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

        if (range < 0)
        {
            return closestObject;
        }
        else if (closestDistance <= range)
        {
            return closestObject;
        }
        return null;
    }
    public void Death()
    {
        agent.isStopped = true;
        IsDead = true;
        StartCoroutine(ShowDeath());
    }

    protected IEnumerator ShowDeath()
    {
        float t = 0.0f;
        float dt = 0.05f;
        float dt2 = 0.01f;
        float alpha = 1.0f;

        while (alpha > 0.0f)
        {
            alpha = Mathf.Lerp(1.0f, 0.0f, t);
            materialLoc.GetComponent<Renderer>().material.SetFloat("Alpha", alpha);

            t += dt * Time.deltaTime;
            dt += dt2;

            yield return new WaitForEndOfFrame();
        }

        HandleShowDeathFinalFrame();
        yield return null;
    }
}
