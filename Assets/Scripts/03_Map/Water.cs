using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour, IInteractable
{
    public ItemData bottle;
    public string GetPromptDesc()
    {
        return "[E] 목을 적시기";
    }

    public string GetPromptName()
    {
        return "오아시스";
    }

    public void SubscribeMethod()
    {
        GameManager.Instance.player.condition.Drink(100);
        Debug.Log("목을 적십니다");
        //물통이 있는지, 있다면 50개 정도로
        if (Inventory.Instance.itemCounts[bottle] < 12)
        {
            Inventory.Instance.itemCounts[bottle] = 12;
            UIManager.Instance.inventoryUI.UpdateUI();
            Debug.Log("물을 채웁니다");
        }
    }
}
