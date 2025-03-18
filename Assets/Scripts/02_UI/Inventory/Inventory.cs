using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<ItemData> testItemAdd = new();
    public List<ItemData> items = new List<ItemData>();
    public ItemData equipitem = new();
    public Dictionary<ItemData, int> itemCounts = new Dictionary<ItemData, int>(); // 아이템 개수 저장
    public int inventorySize = 20;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        //테스트용 아이템 추가
        foreach (ItemData item in testItemAdd)
            AddItem(item);
    }

    public bool AddItem(ItemData newItem)
    {
        if (newItem.canStack && itemCounts.ContainsKey(newItem))
        {
            itemCounts[newItem]++;
            return true;
        }

        if (items.Count < inventorySize)
        {
            items.Add(newItem);
            itemCounts[newItem] = 1;
            return true;
        }

        return false;
    }

    public void RemoveItem(ItemData itemToRemove)
    {
        if (itemCounts.ContainsKey(itemToRemove))
        {
            itemCounts[itemToRemove]--;
            if (itemCounts[itemToRemove] <= 0)
            {
                items.Remove(itemToRemove);
                itemCounts.Remove(itemToRemove);
            }
        }
    }
}
