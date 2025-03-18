using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IImpactable // 에네미, 땅, 트레저, 자원이 상속해야할 인터페이스 
{
    public void ReceiveImpact(float value); 
}

public class Equip_Item : MonoBehaviour // 실제 데이터는 인벤에서 슬롯 또는 item으로 관리되고 있을 테고, 여기서는 공격 행위 자체에 필요한 정보만 있으면 됨(공격력, 사거리, 사용 스태미나)
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
                hitLayerMask = 1 << 8; // 적
                break;
            case EquipableType.DoesDiscover:
                hitLayerMask = 1 << 7; // 땅
                break;
            case EquipableType.DoesDig:
                hitLayerMask = 1 << 10; // 트레저
                break;
            case EquipableType.DoesGatherResources:
                hitLayerMask = 1 << 11; // 자원
                break;
        }
        value = itemData.value;
        useStamina = itemData.useStamina;
        distance = itemData.distance;
        rate = itemData.rate;
    }

    // 히트(무기 - 적, 자원 - 자원, 삽 - 땅 속에 숨겨진 트리거, 돋보기/나침반 - 땅)
    public void StartEquipInteraction() // 행동 시작
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

    public void PerformEquipInteraction() // 히트 판정
    {
        if (Physics.Raycast(cam.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hitinfo, distance, hitLayerMask)) // cam.screenToRay대신 뷰포트로 했는데 잘 될지 테스트해바야함
        {
            Debug.Log($"{hitinfo.collider.name}를 맞춤");
            IImpactable impactable;
            if (hitinfo.collider.TryGetComponent<IImpactable>(out impactable))
            {
                impactable.ReceiveImpact(value);
            }
            else Debug.LogWarning("도구의 대상 오브젝트는 IImpactable 인터페이스를 상속받아야합니다.");
        }
        else Debug.Log("미스~");
    }
}
