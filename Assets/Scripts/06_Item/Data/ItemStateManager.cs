using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ItemStateExtensions
{
    //public static GameObject ToDropItem(this ItemData item2Place) // item2Place.basePref : 위에 빈 부모 클래스 하나 잇는    
    // {
    //    GameObject go = new GameObject($"Drop_{item2Place}");
    //    Object.Instantiate(item2Place.renderPref, go.transform, false);
    //    go.AddComponent<Drop_Item>().item2Place = item2Place;

    //    return go;
    //}


    /// <summary>
    /// 역할: ItemData => Drop_Item 변환
    /// 설명: itemData를 Drop_Item클래스로 감싸서 객체화
    /// </summary>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public static GameObject ToDropItem(this ItemData itemData)
    {
        GameObject go = Object.Instantiate(itemData.basePref);
        go.name = $"Drop_{itemData}";
        go.AddComponent<Drop_Item>().itemData = itemData;
        go.transform.GetChild(0).AddComponent<Rigidbody>();
        ChangeLayer(go, 13);

        return go;
    }

    public static GameObject ToDropItem(this ItemData itemData, Transform parent, bool instantiateInWorldSpace = false)
    {
        GameObject go = itemData.ToDropItem();
        go.transform.SetParent(parent, instantiateInWorldSpace);

        return go;
    }
    public static GameObject ToDropItem(this ItemData itemData, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject go = itemData.ToDropItem();
        go.transform.position = position;
        go.transform.rotation = rotation;
        if(parent != null)
        {
            go.transform.SetParent(parent, false);    
        }

        return go;
    }

    /// <summary>
    /// 역할: ItemData => Placed_Item 변환
    /// 설명: itemData를 Placed_Item클래스로 감싸서 객체화
    /// </summary>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public static GameObject ToPlacedItem(this ItemData itemData, Vector3 worldPosition, Quaternion rotation)
    {
        GameObject go = Object.Instantiate(itemData.basePref);
        go.name = $"Placed_{itemData}";
        PlaceItem_AddScript(go, itemData);

        go.transform.position = worldPosition;
        go.transform.rotation = rotation;

        ChangeLayer(go, 13);

        return go;
    }

    /// <summary>
    /// 역할: ItemData => Equip_Item 변환. 
    /// 설명: itemData를 Equip_Item클래스로 감싸서 객체화
    /// </summary>
    /// <param name="itemData"></param> 
    /// <returns></returns>
    public static GameObject ToEquipItem(this ItemData itemData, Transform parent, bool instantiateInWorldSpace = false)
    {
        GameObject go = Object.Instantiate(itemData.equipPref);
        go.transform.SetParent(parent, instantiateInWorldSpace);

        return go;
    }

    /// <summary>
    /// 역할: ItemData => Equip_Item 변환. 
    /// 설명: itemData를 Equip_Item클래스로 감싸서 객체화
    /// </summary>
    /// <param name="itemData"></param> 
    /// <returns></returns>
    public static GameObject ToPreviewItem(this ItemData itemData, Vector3 worldPosition, Quaternion rotation)
    {
        GameObject go = Object.Instantiate(itemData.basePref);
        go.name = $"Preview_{itemData}";
        go.AddComponent<Preview_Item>().item2Place = itemData;

        go.transform.position = worldPosition;
        go.transform.rotation = rotation;

        return go;
    }

    private static void ChangeLayer(GameObject parent, int layerNum)
    {
        if (parent == null) return;

        parent.layer = layerNum;

        foreach(Transform child in parent.transform)
        {
            ChangeLayer(child.gameObject, layerNum);
        }
    }

    private static void PlaceItem_AddScript(GameObject go, ItemData itemData)
    {

        switch (itemData.item_name)
        {
            case "상자":
                go.AddComponent<Placed_Chest>().itemData = itemData;
                break;
            case "모닥불":
                go.AddComponent<Placed_CampFire>().itemData = itemData;
                break;
            case "텐트":
                go.AddComponent<Placed_Tent>().itemData = itemData;
                break;
            case "침대":
                go.AddComponent<Placed_Bed>().itemData = itemData;
                break;
            case "작업대":
                go.AddComponent<Placed_WorkBench>().itemData = itemData;
                break;
        }
    }
}
