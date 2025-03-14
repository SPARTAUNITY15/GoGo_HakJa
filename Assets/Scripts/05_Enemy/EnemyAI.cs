using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public abstract class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask playerLayer, safeZoneLayer;
    public Animator animator; 
    protected enum State { Patrolling, Chasing, Attacking, Safe, Dead }
    protected State currentState = State.Patrolling;

    public float patrolRange;
    public float sightRange;
    public float attackRange;
    public float safezoneRange;
    public float maxChaseDistance;
    public float health = 100;

    private Vector3 patrolTarget;
    protected bool isPlayerInSight, isPlayerInAttackRange, isInSafeZone;
    protected ItemDropper itemDropper;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        itemDropper = GetComponent<ItemDropper>();

        SetNewPatrolPoint();
    }

    protected virtual void Update()
    {
        if (currentState == State.Dead) return;

        isPlayerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        isInSafeZone = Physics.CheckSphere(transform.position, safezoneRange, safeZoneLayer);

        if (isInSafeZone)
        {
            currentState = State.Safe;
            StopMoving();
        }
        else
        {
            switch (currentState)
            {
                case State.Patrolling:
                    Patrol();
                    if (isPlayerInSight) currentState = State.Chasing;
                    break;

                case State.Chasing:
                    ChasePlayer();
                    if (isPlayerInAttackRange) currentState = State.Attacking;
                    break;

                case State.Attacking:
                    AttackPlayer();
                    if (!isPlayerInAttackRange) currentState = State.Patrolling;
                    break;
            }
        }
    }

    protected void SetNewPatrolPoint()
    {
        Vector3 randomPoint = transform.position + new Vector3(
            Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange));

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
            agent.SetDestination(patrolTarget);
            animator.SetBool("IsMoving", true);
        }
    }

    protected void Patrol()
    {
        if (Vector3.Distance(transform.position, patrolTarget) < 1f)
        {
            SetNewPatrolPoint();
        }
    }

    protected void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > maxChaseDistance)
        {
            currentState = State.Patrolling;
            SetNewPatrolPoint();
        }
        else
        {
            agent.SetDestination(player.position);
            animator.SetBool("IsMoving", true);
        }
    }

    protected virtual void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        animator.SetBool("IsMoving", false);
        animator.SetTrigger("Attack");
    }

    protected void StopMoving()
    {
        agent.SetDestination(transform.position);
        animator.SetBool("IsMoving", false);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        currentState = State.Dead;
        animator.SetTrigger("Die");
        agent.isStopped = true; //네비게이션 중지
        GetComponentInChildren<Collider>().enabled = false;

        if (itemDropper != null)
        {
            itemDropper.DropItemWithDelay(0.5f);
        }

        Destroy(gameObject, 3f);
    }
}

