using UnityEngine;

public class SkeletonAI : EnemyAI
{
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f; //���� ��Ÿ��
    
    private float lastAttackTime = 0f;

    protected override void AttackPlayer()
    {
        if (Time.time - lastAttackTime < attackCooldown) return; 
        lastAttackTime = Time.time;

        base.AttackPlayer(); 
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
}

