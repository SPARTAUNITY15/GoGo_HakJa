using UnityEngine;

public class SkeletonAI : EnemyAI
{
    protected override void AttackPlayer()
    {
        base.AttackPlayer(); // 기본 공격 동작 유지
        Debug.Log("스켈레톤이 검으로 공격!");
    }
}
