using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placed_Item : MonoBehaviour, IInteractable
{
    public ItemData itemData;

    public string GetPromptDesc()
    {
        return itemData.item_description + "\n [G] ��ȣ�ۿ�"; 
    }

    public string GetPromptName()
    {
        return itemData.item_name;
    }

    public virtual void SubscribeMethod()
    {
    }
}
