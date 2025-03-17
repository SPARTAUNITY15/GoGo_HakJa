using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform slotsParent;
    private InventorySlot[] slots;

    private void Start()
    {
        slots = slotsParent.GetComponentsInChildren<InventorySlot>();
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
    }
}
