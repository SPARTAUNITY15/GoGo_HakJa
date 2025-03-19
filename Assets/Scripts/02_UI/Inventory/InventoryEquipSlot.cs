using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryEquipSlot : InventorySlot
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        hoverOnSlot = true;
        if (currentItem != null)
        {
            Tooltip.Instance.ShowTooltip(currentItem, true);
        }
    }
}
