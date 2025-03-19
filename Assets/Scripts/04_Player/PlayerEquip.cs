using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class PlayerEquip : MonoBehaviour
{
    public Transform equipPivot; // 아이템 생성될 곳.(캐릭터 오른손 검지(c_index1.r)로 해둠)
    public Equip_Item equippedItem; // 장착 중인 아이템.
    public bool isEquipping;

    public Action<float> attackAction;
    public Transform cameraTrs;

    public void Equip(ItemData item)
    {
        if (isEquipping)
        {
            Destroy(equippedItem.gameObject);
            equippedItem = null;
        }

        equippedItem = item.ToEquipItem(equipPivot, false).GetComponent<Equip_Item>();
        isEquipping = true;
        //item.isEquiped = true;
        attackAction = equippedItem.PerformEquipInteraction;

        Inventory.Instance.RemoveItem(item);
        UIManager.Instance.inventoryUI.UpdateUI();
    }

    public void Unequip(ItemData item)
    {
        Destroy(equippedItem.gameObject);
        equippedItem = null;
        isEquipping = false;
        //item.isEquiped = false;
        attackAction = null;

        Inventory.Instance.AddItem(item);
        UIManager.Instance.inventoryUI.UpdateUI();
    }

    //public void OnAttack(InputAction.CallbackContext context) // 테스트용 임시 메서드
    //{
    //    if (context.phase == InputActionPhase.Started)
    //    {
    //        attackAction?.Invoke();
    //        //equippedItem.StartEquipInteraction();
    //    }
    //}

    public void OnAttackEvent()
    {
            attackAction?.Invoke(CalcCamera2Player());
    }

    private float CalcCamera2Player()
    {
        return MathF.Abs((transform.position - cameraTrs.position).magnitude);
    }
}
