using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Drop_Item : MonoBehaviour ,IInteractable
{
    public ItemData itemData;

    public void GetPromptInfo()
    {
    }

    public void SubscribeMethod()
    {
        Debug.Log("������ �ݱ�");
    }
}
