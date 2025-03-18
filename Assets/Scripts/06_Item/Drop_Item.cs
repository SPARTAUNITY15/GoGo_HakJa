using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Drop_Item : MonoBehaviour ,IInteractable
{
    public ItemData itemData;

    public string GetPromptDesc()
    {
        return "[G] 줍기";
    }

    public string GetPromptName()
    {
        return itemData.name;
    }

    public void SubscribeMethod()
    {
        Debug.Log("아이템 줍기");
    }
}
