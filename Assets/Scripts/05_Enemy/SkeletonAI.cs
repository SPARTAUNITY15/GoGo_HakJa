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

