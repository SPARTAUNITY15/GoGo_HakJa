using UnityEngine;
using System.Collections;

public class SkeletonAI : EnemyAI
{
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    protected override void AttackPlayer()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;

        Debug.Log("���̷����� ������ ����!");

        DamagePlayer();
    }

    private void DamagePlayer()
    {
        if (player != null)
        {
            StatManager playerStats = player.GetComponent<StatManager>();
            if (playerStats != null)
            {
                playerStats.TakePhysicalDamage(attackDamage);
                Debug.Log($"�÷��̾ {attackDamage}��ŭ ���ظ� ����! ���� ü��: {playerStats.health}");
            }
        }
    }
    protected override void Die()
    {
        currentState = State.Dead;
        agent.isStopped = true;
        GetComponentInChildren<Collider>().enabled = false;

        int randomDeath = Random.Range(0, 2);
        string deathTrigger = randomDeath == 0 ? "Die" : "Die2";

        animator.SetTrigger(deathTrigger);

        if (itemDropper != null)
        {
            itemDropper.DropItemWithDelay(0.5f);
        }

        Destroy(gameObject, 3f);
    }
}

