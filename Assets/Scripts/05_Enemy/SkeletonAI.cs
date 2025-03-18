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

        Debug.Log("스켈레톤이 검으로 공격!");

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
                Debug.Log($"플레이어가 {attackDamage}만큼 피해를 입음! 현재 체력: {playerStats.curHealth}");
            }
        }
    }
}

