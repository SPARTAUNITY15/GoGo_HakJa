using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Drop_Item : MonoBehaviour ,IInteractable
{
    public ItemData itemData;

    public string GetPromptDesc()
    {
        return "[G] �ݱ�";
    }

    public string GetPromptName()
    {
        return itemData.name;
    }

    public void SubscribeMethod()
    {
        Debug.Log("������ �ݱ�");
    }
}
