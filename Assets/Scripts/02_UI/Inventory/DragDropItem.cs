using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Image icon;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        icon = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root); // UI 최상위로 이동
        canvasGroup.blocksRaycasts = false; // 드래그 중 Raycast 무시
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position; // 마우스를 따라 이동
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject targetSlot = eventData.pointerEnter;

        if (targetSlot != null && targetSlot.GetComponent<InventorySlot>() != null)
        {
            transform.SetParent(targetSlot.transform);
        }
        else
        {
            transform.SetParent(originalParent);
        }

        rectTransform.localPosition = Vector3.zero; // 슬롯 중앙 정렬
        canvasGroup.blocksRaycasts = true;
    }
}
