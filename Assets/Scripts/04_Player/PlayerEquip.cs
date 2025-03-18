using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
    public Transform equipPivot; // ������ ������ ��.(ĳ���� ������ ����(c_index1.r)�� �ص�)
    public Equip_Item equippedItem; // ���� ���� ������.
    public bool isEquipping;

    public void Equip(ItemData item)
    {
        if (isEquipping)
        {
            equippedItem = null;
        }

        equippedItem = item.ToEquipItem(equipPivot, false).GetComponent<Equip_Item>();
        isEquipping = true;
    }

    public void Unequip()
    {
        equippedItem = null;
        isEquipping = false;
    }
}
