using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask playerLayer, safeZoneLayer; //groundLayer
    public Animator animator;
    private enum State 
    { 
        Patrolling, 
        Chasing, 
        Attacking, 
        Safe,
        Dead
    }
    private State currentState = State.Patrolling;

    public float patrolRange;
    public float sightRange;
    public float attackRange;
    public float safezoneRange;
    public float maxChaseDistance;
    public float health = 100; 

    private Vector3 patrolTarget;
    private bool isPlayerInSight, isPlayerInAttackRange, isInSafeZone;
    private ItemDropper itemDropper;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        itemDropper = GetComponent<ItemDropper>();

        SetNewPatrolPoint();
        Die();
    }

    void Update()
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

    void SetNewPatrolPoint()
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

    void Patrol()
    {
        if (Vector3.Distance(transform.position, patrolTarget) < 1f)
        {
            SetNewPatrolPoint();
        }
    }

    void ChasePlayer()
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

    void AttackPlayer()
    {
        agent.SetDestination(transform.position); // 공격 시 멈춤
        animator.SetBool("IsMoving", false);
        animator.SetTrigger("Attack");
    }

    void StopMoving()
    {
        agent.SetDestination(transform.position);
        animator.SetBool("IsMoving", false);  // 안전구역에서는 멈춤
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        currentState = State.Dead;
        animator.SetTrigger("Die");
        agent.isStopped = true; // 네비게이션 중지
        GetComponentInChildren<Collider>().enabled = false;

        if (itemDropper != null)
        {
            ///아이템 드롭
            //itemDropper.DropItem(); 
            itemDropper.DropItemWithDelay(0.5f);
        }
        else if (itemDropper == null)
        {
            Debug.Log("itemDropper가 Null입니다.");
        }

        Destroy(gameObject, 3f);
    }
}
