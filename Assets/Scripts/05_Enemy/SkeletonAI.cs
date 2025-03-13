using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask groundLayer, playerLayer, safeZoneLayer;
    public Animator animator;
    private enum State { Patrolling, Chasing, Attacking, Safe }
    private State currentState = State.Patrolling;

    public float patrolRange = 10f;
    public float sightRange = 15f;
    public float attackRange;
    public float safeZoneCheckRadius = 3f;

    private Vector3 patrolTarget;
    private bool isPlayerInSight, isPlayerInAttackRange, isInSafeZone;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // 하이어라키에서 Player 태그를 가진 오브젝트를 찾아서 player에 할당
        player = GameObject.FindGameObjectWithTag("Player").transform;

        SetNewPatrolPoint();
    }

    void Update()
    {
        isPlayerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        isInSafeZone = Physics.CheckSphere(transform.position, safeZoneCheckRadius, safeZoneLayer);

        if (isInSafeZone)
        {
            currentState = State.Safe;
            StopMoving();  // 안전구역에서는 멈춤
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
            animator.SetBool("IsMoving", true);  //  이동 애니메이션 활성화
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
        agent.SetDestination(player.position);
        animator.SetBool("IsMoving", true);  //  추격 시 걷기 애니메이션
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position); // 공격 시 멈춤
        //transform.LookAt(player); //플레이어 바라보기 (Rotation값 회전이 있어서 우선 비활성화)
        animator.SetBool("IsMoving", false);
        animator.SetTrigger("Attack");
        Debug.Log("공격!");
    }

    void StopMoving()
    {
        agent.SetDestination(transform.position);
        animator.SetBool("IsMoving", false);  // 안전구역에서는 멈춤
    }
}
