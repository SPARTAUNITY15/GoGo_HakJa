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

        animator.SetTrigger("Attack");

        Debug.Log("���̷����� ������ ����!");

        DamagePlayer();
    }

    private void DamagePlayer()
    {
        if (player != null)
        {
            PlayerCondition playerStats = player.GetComponent<PlayerCondition>();
            if (playerStats != null)
            {
                playerStats.TakePhysicalDamage(attackDamage);
                Debug.Log($"�÷��̾ {attackDamage}��ŭ ���ظ� ����! ���� ü��: {playerStats.curHealth}");
            }
        }
    }
}

