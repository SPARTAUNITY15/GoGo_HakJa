using UnityEngine;

public class SpiderAI : EnemyAI
{
    public GameObject projectilePrefab;
    public Transform attackPoint;
    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    protected override void AttackPlayer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            RangedAttack();
            lastAttackTime = Time.time;
        }
    }

    private void RangedAttack()
    {
        if (projectilePrefab != null && attackPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);
            Projectile projScript = projectile.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.spiderAI = this; 

                Vector3 direction = (player.position - attackPoint.position).normalized;
                projScript.SetTarget(direction);
            }
        }

        animator.SetTrigger("Attack");
        //DamagePlayer();
        Debug.Log("�Ź̰� ����ü ����!");
    }

    public void DamagePlayer()
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

