using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
    public Transform equipPivot; // 아이템 생성될 곳.(캐릭터 오른손 검지(c_index1.r)로 해둠)
    public Equip_Item equippedItem; // 장착 중인 아이템.
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
