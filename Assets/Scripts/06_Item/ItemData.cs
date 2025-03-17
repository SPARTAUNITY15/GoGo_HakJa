




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Placeable,
    Resources

}

//[System.Serializable]
//public class ItemData_ItemType
//{
//    public ItemType itemType;
//}

public enum ConsumableType
{
    Health,
    Stamina,
    Hunger,
    Thirst,
    Temperature
}

[System.Serializable]
public class ItemData_Consumable
{
    public ConsumableType consumableType;
    public float value;
}

public enum EquipableType
{
    DoesGatherResources,
    DoesAttack,
    DoesShoot,
    DoesDig,
    DoesDiscover
    //DoesStat
}

//public enum DoesStatType // ���߿� ���Ϸ���
//{
//    MoveSpeed,
//    JumpPower,
//    MaxHealth,
//}


//[System.Serializable]
//public class ItemData_Equipable
//{
//    public EquipableType equipableType;
//}

public enum PlaceableType
{
    Sleep,
    Craft,
    Tent,
    Storage,

}

//[System.Serializable]
//public class ItemData_Placeable
//{
//    public PlaceableType placeableType;
//}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("���� ����")]
    public ItemType itemType;
    //public ItemData_ItemType itemData_ItemType;
    public string item_name;
    public string item_description;

    public bool canStack;
    public int maxStack = 12;
    //public GameObject renderPref;
    public GameObject basePref;
    public Sprite Icon;
    public bool isNotStackOver; // ������ 0�� �Ǿ �Ȼ����.

    [Header("�Һ���")]
    public ItemData_Consumable[] ItemData_Consumables;
    public bool isCookable;
    public ItemData CookedThing;

    [Header("�����")]
    //public ItemData_Equipable[] itemData_Equipables; 
    public EquipableType equipableType;
    public float value; // �ڿ� - �ѹ��� ��� �ڿ��� Ķ��, ���� - ������. �� �ܿ��� ��� ������ ���� ���� ����..
    public float useStamina; // ��� ���¹̳�
    public float distance;
    public float rate; // ���� ���ð�
    public bool isEquiped;
    public GameObject equipPref;

    //[Header("��ġ��")]
    //public ItemData_Placeable[] itemData_Placeable;
}
