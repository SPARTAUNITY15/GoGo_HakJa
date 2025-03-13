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

        // ���̾��Ű���� Player �±׸� ���� ������Ʈ�� ã�Ƽ� player�� �Ҵ�
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
            StopMoving();  // �������������� ����
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
            animator.SetBool("IsMoving", true);  //  �̵� �ִϸ��̼� Ȱ��ȭ
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
        animator.SetBool("IsMoving", true);  //  �߰� �� �ȱ� �ִϸ��̼�
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position); // ���� �� ����
        //transform.LookAt(player); //�÷��̾� �ٶ󺸱� (Rotation�� ȸ���� �־ �켱 ��Ȱ��ȭ)
        animator.SetBool("IsMoving", false);
        animator.SetTrigger("Attack");
        Debug.Log("����!");
    }

    void StopMoving()
    {
        agent.SetDestination(transform.position);
        animator.SetBool("IsMoving", false);  // �������������� ����
    }
}
