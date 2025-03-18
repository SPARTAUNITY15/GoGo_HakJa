using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CraftMode
{
    Campfire,
    WorkBench,
    Inventory
}

[System.Serializable]
public class TableDict
{
    public string name;
    public GameObject table;
}

public class InventoryUI : MonoBehaviour
{
    public Transform slotsParent;
    private InventorySlot[] slots;
    public InventorySlot equipSlot;

    //public Dictionary<string, CraftingTable> tableDict;
    public List<TableDict> tableClassLists;
    public Dictionary<string, GameObject> tableDictionary = new();
    public CraftMode curCraftMode;

    void Awake()
    {
        slots = slotsParent.GetComponentsInChildren<InventorySlot>();

        foreach (var table in tableClassLists)
        {
            tableDictionary.Add(table.name, Instantiate(table.table, transform));
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.Instance.items.Count)
            {
                ItemData item = Inventory.Instance.items[i];
                int count = Inventory.Instance.itemCounts[item];
                slots[i].SetItem(item, count);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

        if (GameManager.Instance.player.playerEquip.isEquipping)
        {
            equipSlot.SetItem(GameManager.Instance.player.playerEquip.equippedItem.itemData, 1);
        }
        else
        {
            equipSlot.ClearSlot();
        }

    }

    public void SetCraftMode(CraftMode newMode)
    {
        curCraftMode = newMode;
        foreach (var i in tableDictionary)
        {
            if (i.Key == curCraftMode.ToString())
            {
                i.Value.gameObject.SetActive(true);
            }
            else
            {
                i.Value.gameObject.SetActive(false);
            }
        }
    }
}
