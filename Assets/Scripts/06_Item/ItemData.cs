




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

//public enum DoesStatType // 나중에 쓰일려나
//{
//    MoveSpeed,
//    JumpPower,
//    MaxHealth,
//}


[System.Serializable]
public class ItemData_Equipable
{
    public EquipableType equipableType;
    public float value; // 자원 - 한번에 몇개의 자원을 캘지, 공격 - 데미지. 그 외에는 무쓸모.
}

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
    [Header("공통 정보")]
    public ItemType itemType;
    //public ItemData_ItemType itemData_ItemType;
    public string item_name;
    public string item_description;

    public bool canStack;
    public int maxStack = 12;
    public GameObject dropPref;
    public Sprite Icon;
    public bool isNotStackOver; // 스택이 0이 되어도 안사라짐.

    [Header("소비템")]
    public ItemData_Consumable[] ItemData_Consumables;
    public bool isCookable;
    public ItemData CookedThing;

    [Header("장비템")]
    public ItemData_Equipable[] itemData_Equipables;
    public GameObject EquipPref;

    [Header("설치템")]
    //public ItemData_Placeable[] itemData_Placeable;
    public GameObject PlacePref;
}
