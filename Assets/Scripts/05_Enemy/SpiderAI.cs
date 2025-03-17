using UnityEngine;

public class SpiderAI : EnemyAI
{
    public GameObject projectilePrefab;
    public Transform attackPoint;
    public float attackCooldown = 2f;

    private float lastAttackTime = 0f;

    protected override void AttackPlayer()
    {
        //base.AttackPlayer();
        //Debug.Log("거미가 투사체 공격!");

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
                Vector3 direction = (player.position - attackPoint.position).normalized;
                projScript.SetTarget(direction);
            }
        }

        animator.SetTrigger("Attack");
        Debug.Log("거미가 투사체 공격!");
    }
}

