using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonScript : MinionScript
{
    [Header("Demon Settings")]
    public float maxHealth = 100.0f;
    public float DetectRadius = 120.5f;
    public float AttackRadius = 2.5f;
    public float Damage = 50.0f;
    public float maxLifetime = 30.0f;

    private enum AIState { DETECT, SELECTED, ATTACK };
    private AIState currentState;
    private GameObject myHuntTarget;
    private float health;
    private float lifetime;
    private SphereCollider attackSphere;

    // Start is called before the first frame update
    protected override void Start()
    {
        attackSphere = GetComponentInChildren<SphereCollider>();
        GetComponentInChildren<Animator>().SetTrigger("IsSpawned");
        health = maxHealth;
        lifetime = maxLifetime;
        base.Start();
    }

    protected override void Update()
    {
        GetComponentInChildren<Animator>()?.SetBool("IsMoving", !IsAgentFinished(0.25f));
        StateBasedUpdate();

        lifetime -= Time.deltaTime;
        GameManager.instance.SetRockHealth(lifetime / maxLifetime);
        if(lifetime <= 0 && !IsDead)
        {
            IsDead = true;
            GetComponentInChildren<Animator>()?.SetTrigger("IsDead");
        }

        base.Update();
    }

    protected override void PlayMovement()
    {
        AudioAgent agent = GetComponent<VolumeAudioAgent>();
        if (agent.IsAudioStopped("RockBoiMove"))
        {
            agent.PlaySoundEffect("RockBoiMove");
        }
    }

    public void PlaySpawnAudio()
    {
        GetComponent<VolumeAudioAgent>().PlaySoundEffect("RockBoiSlam");
    }

    private void StateBasedUpdate()
    {
        if (IsDead)
        {
            return;
        }

        if (IsSelected)
        {
            //TransitionTo(AIState.SELECTED
            IsSelected = false;
        }
        else if (currentState == AIState.SELECTED && IsAgentFinished())
        {
            agent.isStopped = true;
            //GetComponent<AudioAgent>().StopAudio("RockBoiMove");
            TransitionTo(AIState.DETECT);
        }

        switch (currentState)
        {
            case AIState.DETECT:
                {
                    DetectUpdate();
                    break;
                }
            case AIState.SELECTED:
                {
                    //SelectedUpdate();
                    break;
                }
            case AIState.ATTACK:
                {
                    AttackUpdate();
                    break;
                }
            default:
                break;
        }
    }

    /*--------------------
     * Detect Mode
     * -------------------
     * The soul intent of this mode is to constantly look out for the nearest enemy to target.
     * Once a blood is within range, which is returned by the FindClosestofTag function, it will transition 
     * to the HARVEST Mode.
     */
    private void DetectUpdate()
    {
        myHuntTarget = FindClosestofTag("Enemy");

        if(myHuntTarget != null)
        {
            SetTargetLocation(myHuntTarget.transform.position);

            if (IsAgentFinished(2.5f))
            {
                TransitionTo(AIState.ATTACK);
            }
        }
    }

    /*--------------------
     * Selected Mode
     * -------------------
     * The soul intent of this state is to allow the player resume full control of this
     * minion. Where the minion will attack any enemies that are near-by.
     */
    private void SelectedUpdate()
    {
        if (IsAgentFinished())
        {
            agent.isStopped = true;
            GetComponent<AudioAgent>().StopAudio("RockBoiMove");
            myHuntTarget = FindClosestofTag("Enemy", DetectRadius);
            GetComponentInChildren<Animator>()?.SetBool("IsAttacking", false);

            if (myHuntTarget != null)
            {
                float distance = Vector3.Distance(transform.position, myHuntTarget.transform.position);
                if (distance > DetectRadius)
                {
                    //Distance is too far, forget about this one!
                    myHuntTarget = null;
                    GetComponentInChildren<Animator>()?.SetBool("IsAttacking", false);
                }
                else if (distance > AttackRadius)
                {
                    //Do nothing, it isn't close enough
                }
                else
                {
                    agent.isStopped = true;
                    GetComponent<AudioAgent>().StopAudio("RockBoiMove");
                    transform.LookAt(myHuntTarget.transform, Vector3.up);
                    transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
                    GetComponentInChildren<Animator>()?.SetBool("IsAttacking", true);
                }
            }
        }
        else
        {
            GetComponentInChildren<Animator>()?.SetBool("IsAttacking", false);
        }
    }

    /*--------------------
     * Deposit Mode
     * -------------------
     * The soul intent of this state is to attack and push back all enemies in an area.
     */
    private void AttackUpdate()
    {
        if(myHuntTarget != null)
        {
            float distance = Vector3.Distance(transform.position, myHuntTarget.transform.position);

            if(distance <= AttackRadius)
            {
                agent.isStopped = true;
                GetComponent<AudioAgent>().StopAudio("RockBoiMove");
                transform.LookAt(myHuntTarget.transform, Vector3.up);
                transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
                GetComponentInChildren<Animator>()?.SetBool("IsAttacking", true);
            }
            else if(distance > DetectRadius)
            {
                TransitionTo(AIState.DETECT);
            }
            else
            {
                SetTargetLocation(myHuntTarget.transform.position);
            }
        }
        else
        {
            TransitionTo(AIState.DETECT);
        }
    }

    private void TransitionTo(AIState newState)
    {
        if (currentState == newState)
            return;

        switch (newState)
        {
            case AIState.DETECT:
                GetComponentInChildren<Animator>()?.SetBool("IsAttacking", false);
                myHuntTarget = null;
                break;
            case AIState.SELECTED:
                GetComponentInChildren<Animator>()?.SetBool("IsAttacking", false);
                myHuntTarget = null;
                break;
            case AIState.ATTACK:
                break;
            default:
                break;
        }

        currentState = newState;
    }

    public void DealDamage()
    {
        Collider[] hits = Physics.OverlapSphere(attackSphere.gameObject.transform.position, attackSphere.radius);
        foreach (var hit in hits)
        {
            if(hit.tag == "Enemy")
            {
                EnemyScript script = hit.gameObject.GetComponentInParent<EnemyScript>();
                script.SetMovementMod(0.5f);
                script.DealDamageToEnemy(Damage);
                StartCoroutine(ResetSlowOn(script, 0.5f));
            }
        }
    }

    override protected GameObject FindClosestofTag(string tag, float range = -1)
    {
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(tag);
        float closestDistance = 100000;
        GameObject closestObject = null;
        
        foreach (var foundObject in foundObjects)
        {
            float distance = Vector3.Distance(transform.position, foundObject.transform.position);

            if (tag == "Enemy" && foundObject.GetComponentInParent<EnemyScript>().IsDead)
            {
                continue;
            }

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

    private IEnumerator ResetSlowOn(EnemyScript enemy, float time)
    {
        float delay = 0;

        while(delay < time)
        {
            delay += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        enemy.SetMovementMod(1.0f);
        yield return null;
    }

    public override void TakeDamage(float damage)
    {
        if (IsDead)
            return;

        health -= damage;
        if(health <= 0)
        {
            IsDead = true;
            GetComponentInChildren<Animator>()?.SetTrigger("IsDead");
        }
    }

    protected override void HandleShowDeathFinalFrame()
    {
        Destroy(gameObject);
    }
}
