using UnityEngine;

public class SkeletonAI : EnemyAI
{
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f; //공격 쿨타임
    
    private float lastAttackTime = 0f;

    protected override void AttackPlayer()
    {
        if (Time.time - lastAttackTime < attackCooldown) return; 
        lastAttackTime = Time.time;

        base.AttackPlayer(); 
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

