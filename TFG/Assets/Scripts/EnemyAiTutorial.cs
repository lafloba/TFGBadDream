using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    public Animator ani;

    private int maxHealth = 100;
    private int currentHealth;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsPlayer;

    // Patroling
    public float walkPointRange;
    private Vector3 walkPoint;
    private bool walkPointSet;

    // States
    public float sightRange;
    public bool playerInSightRange;

    // Speed settings
    public float patrolSpeed = 1f;
    public float chaseSpeed = 4f;

    // Patroling settings
    public float minPatrolWaitTime = 1f;
    public float maxPatrolWaitTime = 3f;
    private float patrolWaitTime;

    void Start()
    {
        ani = GetComponent<Animator>();
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        agent.autoBraking = false;
        agent.stoppingDistance = 1.5f;
        walkPointSet = false;
        walkPoint = Vector3.zero;
        patrolWaitTime = Random.Range(minPatrolWaitTime, maxPatrolWaitTime);
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!playerInSightRange)
        {
            Patroling();
        }
        else
        {
            agent.speed = chaseSpeed;
            ChasePlayer();
        }

        UpdateAnimations();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Monster hit by flashlight! Remaining health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Monster has died!");
        Destroy(gameObject);
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        else
        {
            agent.SetDestination(walkPoint);
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (patrolWaitTime <= 0f)
                {
                    walkPointSet = false;
                    patrolWaitTime = Random.Range(minPatrolWaitTime, maxPatrolWaitTime);
                }
                else
                {
                    patrolWaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void SearchWalkPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkPointRange;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, walkPointRange, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }

    private void UpdateAnimations()
    {
        bool isWalking = agent.velocity.magnitude > 0.1f;
        ani.SetBool("walk", isWalking);
    }
}
