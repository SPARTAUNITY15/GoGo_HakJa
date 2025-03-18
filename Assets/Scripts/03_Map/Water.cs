using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour, IInteractable
{
    public ItemData bottle;
    public string GetPromptDesc()
    {
        return "[E] ���� ���ñ�";
    }

    public string GetPromptName()
    {
        return "���ƽý�";
    }

    public void SubscribeMethod()
    {
        GameManager.Instance.player.condition.Drink(100);
        Debug.Log("���� ���ʴϴ�");
        //������ �ִ���, �ִٸ� 50�� ������
        if (Inventory.Instance.itemCounts[bottle] < 12)
        {
            Inventory.Instance.itemCounts[bottle] = 12;
            UIManager.Instance.inventoryUI.UpdateUI();
            Debug.Log("���� ä��ϴ�");
        }
    }
}
