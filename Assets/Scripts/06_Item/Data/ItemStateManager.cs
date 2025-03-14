using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemStateExtensions
{
    //public static GameObject ToDropItem(this ItemData itemData) // itemData.basePref : 위에 빈 부모 클래스 하나 잇는    
    // {
    //    GameObject go = new GameObject($"Drop_{itemData}");
    //    Object.Instantiate(itemData.renderPref, go.transform, false);
    //    go.AddComponent<Drop_Item>().itemData = itemData;

    //    return go;
    //}

    public static GameObject ToDropItem(this ItemData itemData)
    {
        GameObject go = Object.Instantiate(itemData.basePref);
        go.name = $"Drop_{itemData}";
        go.AddComponent<Drop_Item>().itemData = itemData;

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

    public static GameObject ToPlacedItem(this ItemData itemData, Vector3 worldPosition, Quaternion rotation)
    {
        GameObject go = new GameObject($"Placed_{itemData}");
        Object.Instantiate(itemData.renderPref, go.transform, false);
        go.AddComponent<Placed_Item>().itemData = itemData;

        go.transform.position = worldPosition;
        go.transform.rotation = rotation;

        return go;
    }


}
