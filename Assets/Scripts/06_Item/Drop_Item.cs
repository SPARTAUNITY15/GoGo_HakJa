using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Drop_Item : MonoBehaviour, IInteractable
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
        ToInventory();
    }

    private void ToInventory()
    {
        if (Inventory.Instance.AddItem(itemData))
        {
            Destroy(gameObject);
            UIManager.Instance.inventoryUI.UpdateUI();
        }
        else { Debug.Log("인벤토리가 가득 찼습니다."); }
    }
}
