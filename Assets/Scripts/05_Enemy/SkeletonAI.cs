using UnityEngine;

public class SkeletonAI : EnemyAI
{
    protected override void AttackPlayer()
    {
        base.AttackPlayer(); // �⺻ ���� ���� ����
        Debug.Log("���̷����� ������ ����!");
    }
}
