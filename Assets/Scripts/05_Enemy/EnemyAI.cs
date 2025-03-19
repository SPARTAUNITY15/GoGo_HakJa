using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public abstract class EnemyAI : MonoBehaviour, IImpactable
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask playerLayer;
    public Animator animator;
    protected enum State { Patrolling, Chasing, Attacking, Safe, Dead }
    protected State currentState = State.Patrolling;

    public float patrolRange;
    public float sightRange;
    public float attackRange;
    public float safezoneRange;
    public float maxChaseDistance;
    public float health = 100;
    public float attackDamage;

    public bool TestDie = false;

    private Vector3 patrolTarget;
    protected bool isPlayerInSight, isPlayerInAttackRange, isInSafeZone;
    protected ItemDropper itemDropper;
    protected SafeZone safeZone;


    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        itemDropper = GetComponent<ItemDropper>();
        safeZone = FindObjectOfType<SafeZone>();

        SetNewPatrolPoint();

    }

    protected virtual void Update()
    {
        // Die 테스트용 함수
        if (TestDie == true)
        {
            Die();
        }

        //TakeDamage 테스트용 함수
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(10);
        }

        if (currentState == State.Dead) return;

        // 플레이어 감지 및 상태 확인
        UpdatePlayerDetection();

        // 상태 전환 관리
        HandleState();

    }
    private void UpdatePlayerDetection()
    {
        isPlayerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        //isInSafeZone = safeZone != null && safeZone.isPlayerInside;

        //if (safeZone == null)
        //{
        //    Debug.LogError("safeZone이 할당되지 않았습니다!");
        //    return;
        //}

        // 안전구역에 있을 경우, 플레이어 감지를 비활성화
        //if (isInSafeZone)
        //{
        //    isPlayerInSight = false;
        //    isPlayerInAttackRange = false;
        //}
    }
    private void HandleState()
    {
        if (isInSafeZone && (currentState == State.Chasing || currentState == State.Attacking))
        {
            TransitionToPatrolling();
            animator.ResetTrigger("Attack");
            return;
        }

        // 상태 전환 처리
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                if (isPlayerInSight) TransitionToChasing();
                break;

            case State.Chasing:
                ChasePlayer();
                if (isPlayerInAttackRange) TransitionToAttacking();
                break;

            case State.Attacking:
                AttackPlayer();
                if (!isPlayerInAttackRange) TransitionToPatrolling();
                break;
        }
    }
    private void TransitionToPatrolling()
    {
        currentState = State.Patrolling;
        SetNewPatrolPoint();
    }

    private void TransitionToChasing()
    {
        currentState = State.Chasing;
    }

    private void TransitionToAttacking()
    {
        currentState = State.Attacking;
    }
    protected void SetNewPatrolPoint()
    {
        Vector3 randomPoint = transform.position + new Vector3(
            Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange));

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
            Debug.Log(patrolTarget);
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

    public void TakeDamage(float damage)
    {
        health -= damage;
        GetComponent<EnemyDamaged>()?.FlashDamage();

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

    public void ReceiveImpact(float value)
    {
        TakeDamage(value);
        Debug.Log(value);
    }
}

