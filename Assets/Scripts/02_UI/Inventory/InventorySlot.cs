using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public TMP_Text stackText; // 스택 개수 표시
    protected ItemData currentItem;
    private int itemCount;

    public void SetItem(ItemData item, int count)
    {
        currentItem = item;
        itemCount = count;
        icon.sprite = item.Icon;
        icon.enabled = true;
        UpdateStackText();
    }

    public void ClearSlot()
    {
        currentItem = null;
        itemCount = 0;
        icon.sprite = null;
        icon.enabled = false;
        stackText.text = "";
    }

    private void UpdateStackText()
    {
        if (currentItem != null && currentItem.canStack && itemCount > 1)
        {
            stackText.text = itemCount.ToString();
        }
        else
        {
            stackText.text = "";
        }
    }

    // 현재 슬롯의 아이템을 반환하는 메서드 추가
    public ItemData GetItem()
    {
        return currentItem;
    }

    public static bool hoverOnSlot;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        hoverOnSlot = true;
        if (currentItem != null)
        {
            Tooltip.Instance.ShowTooltip(currentItem, false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverOnSlot = false;
        Tooltip.Instance.coroutine = StartCoroutine(Tooltip.Instance.HideTooltip());
    }
}
