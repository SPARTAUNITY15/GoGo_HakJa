using UnityEngine;

public class SpiderAI : EnemyAI
{
    protected override void AttackPlayer()
    {
        base.AttackPlayer();
        Debug.Log("거미가 투사체 공격!");
        
    }
}

