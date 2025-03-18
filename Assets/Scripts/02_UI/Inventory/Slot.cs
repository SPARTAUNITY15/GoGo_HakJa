using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public SlotType slotType;

    public void OnDrop(PointerEventData eventData)
    {
        DragDropItem droppedItem = eventData.pointerDrag.GetComponent<DragDropItem>();
        if (droppedItem == null) return;

        // 📌 드롭된 아이템의 InventorySlot 컴포넌트 가져오기
        InventorySlot droppedSlot = droppedItem.GetComponent<InventorySlot>();
        if (droppedSlot == null) return;

        ItemData droppedItemData = droppedSlot.GetItem();
        if (droppedItemData == null) return;

        // 슬롯 타입 확인 (일반 슬롯인지, 장비 슬롯인지)
        if (slotType == SlotType.Equipment && droppedItemData.itemType != ItemType.Equipable)
        {
            return; // 장비 슬롯에 일반 아이템을 놓을 수 없음
        }

        // 현재 슬롯에 있는 InventorySlot 가져오기
        InventorySlot currentSlot = GetComponent<InventorySlot>();
        if (currentSlot == null) return;

        if (currentSlot.GetItem() != null)
        {
            // 기존 아이템과 스왑 처리
            ItemData tempItem = currentSlot.GetItem();
            int tempCount = Inventory.Instance.itemCounts[tempItem];

            currentSlot.SetItem(droppedItemData, Inventory.Instance.itemCounts[droppedItemData]);
            droppedSlot.SetItem(tempItem, tempCount);
        }
        else
        {
            currentSlot.SetItem(droppedItemData, Inventory.Instance.itemCounts[droppedItemData]);
            droppedSlot.ClearSlot();
        }

        droppedItem.transform.SetParent(transform);
        droppedItem.transform.localPosition = Vector3.zero;
    }
}

public enum SlotType
{
    Inventory, // 일반 인벤토리 슬롯
    Equipment  // 장비 슬롯
}
