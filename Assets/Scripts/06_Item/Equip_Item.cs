using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IImpactable // ���׹�, ��, Ʈ����, �ڿ��� ����ؾ��� �������̽� 
{
    public void ReceiveImpact(float value); 
}

public class Equip_Item : MonoBehaviour // ���� �����ʹ� �κ����� ���� �Ǵ� item���� �����ǰ� ���� �װ�, ���⼭�� ���� ���� ��ü�� �ʿ��� ������ ������ ��(���ݷ�, ��Ÿ�, ��� ���¹̳�)
{
    Camera cam;
    Animator animator;
    private PlayerCondition playerCondition;

    public ItemData itemData;

    EquipableType equipableType;
    LayerMask hitLayerMask;

    float value;
    float useStamina;
    float distance;
    float rate;

    bool isAttacking;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerCondition = FindObjectOfType<PlayerCondition>();
        cam = Camera.main;

        equipableType = itemData.equipableType;
        switch (equipableType)
        {
            case EquipableType.DoesAttack:
                hitLayerMask = 1 << 8; // ��
                break;
            case EquipableType.DoesDiscover:
                hitLayerMask = 1 << 7; // ��
                break;
            case EquipableType.DoesDig:
                hitLayerMask = 1 << 10; // Ʈ����
                break;
            case EquipableType.DoesGatherResources:
                hitLayerMask = 1 << 11; // �ڿ�
                break;
        }
        value = itemData.value;
        useStamina = itemData.useStamina;
        distance = itemData.distance;
        rate = itemData.rate;
    }

    // ��Ʈ(���� - ��, �ڿ� - �ڿ�, �� - �� �ӿ� ������ Ʈ����, ������/��ħ�� - ��)
    public void StartEquipInteraction() // �ൿ ����
    {
        if (!isAttacking)
        {
            if(!isAttacking && playerCondition.UseStamina(useStamina))
            {
                isAttacking = true;
                animator.SetTrigger("Interaction");
                Invoke("OnCanAttack", rate);
            }
        }
    }

    private void OnCanAttack()
    {
        isAttacking = false;
    }

    public void PerformEquipInteraction() // ��Ʈ ����
    {
        if (Physics.Raycast(cam.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hitinfo, distance, hitLayerMask)) // cam.screenToRay��� ����Ʈ�� �ߴµ� �� ���� �׽�Ʈ�عپ���
        {
            Debug.Log($"{hitinfo.collider.name}�� ����");
            IImpactable impactable;
            if (hitinfo.collider.TryGetComponent<IImpactable>(out impactable))
            {
                impactable.ReceiveImpact(value);
            }
            else Debug.LogWarning("������ ��� ������Ʈ�� IImpactable �������̽��� ��ӹ޾ƾ��մϴ�.");
        }
        else Debug.Log("�̽�~");
    }
}
