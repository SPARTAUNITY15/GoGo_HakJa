using UnityEngine;
using System.Collections;

public class SkeletonAI : EnemyAI
{
    [SerializeField] private float attackDamage = 10f;

    public float attackCooldown;
    private float lastAttackTime = 0f;

    protected override void AttackPlayer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            base.AttackPlayer();
            lastAttackTime = Time.time;
        }
        Debug.Log("스켈레톤이 검으로 공격!");

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
                Debug.Log($"플레이어가 {attackDamage}만큼 피해를 입음! 현재 체력: {playerStats.health}");
            }
        }
    }
}

