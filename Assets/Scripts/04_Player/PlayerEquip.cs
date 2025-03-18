using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class PlayerEquip : MonoBehaviour
{
    public Transform equipPivot; // ������ ������ ��.(ĳ���� ������ ����(c_index1.r)�� �ص�)
    public Equip_Item equippedItem; // ���� ���� ������.
    public bool isEquipping;

    public Action attackAction;

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
        attackAction = equippedItem.StartEquipInteraction;

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

    public void OnAttackInput(InputAction.CallbackContext context) // �׽�Ʈ�� �ӽ� �޼���
    {
        if (context.phase == InputActionPhase.Started)
        {
            attackAction?.Invoke();
            //equippedItem.StartEquipInteraction();
        }
    }
}
