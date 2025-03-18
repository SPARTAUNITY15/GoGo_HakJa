using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static Tooltip Instance;
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipName;
    public TextMeshProUGUI tooltipDesc;
    public Image Image;
    private RectTransform rectTransform;
    //private Vector3 offset = new Vector3(150f, -50f, 0f); // 마우스와의 거리 조정
    private Vector3 offset = Vector3.zero; // 마우스와의 거리 조정

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        rectTransform = tooltipPanel.GetComponent<RectTransform>();
        tooltipPanel.SetActive(false);
    }

    //private void Update()
    //{
    //    if (tooltipPanel.activeSelf)
    //    {
    //        // 툴팁 위치를 마우스 커서 근처로 이동
    //        Vector3 newPos = Input.mousePosition + offset;
    //        newPos.z = 0f;
    //        rectTransform.position = newPos;
    //    }
    //}

    void SetPos()
    {
        // 툴팁 위치를 마우스 커서 근처로 이동
        Vector3 newPos = Input.mousePosition + offset;
        newPos.z = 0f;
        rectTransform.position = newPos;
    }

    //public void ShowTooltip(string name, string description)
    //{
    //    tooltipPanel.SetActive(true);
    //    tooltipName.text = name;
    //    tooltipDesc.text = description;
    //}

    //public void HideTooltip()
    //{
    //    tooltipPanel.SetActive(false);
    //}

    public Coroutine coroutine;
    public static bool hoverOnTooltip;
    public void ShowTooltip(string name, string description, ItemData item)
    {
        tooltipPanel.SetActive(true);
        ShowButton(item);
        SetPos();
        tooltipName.text = name;
        tooltipDesc.text = description;
        coroutine = null;

    }

    public IEnumerator HideTooltip()
    {
        yield return new WaitForSeconds(0.5f);
        if (!hoverOnTooltip && !InventorySlot.hoverOnSlot)
            tooltipPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverOnTooltip = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverOnTooltip = false;
        StartCoroutine(HideTooltip());
    }

    public Button EquipBtn;
    public Button UnequipBtn;
    public Button UseBtn;
    public Button PlaceBtn;
    public Button DropBtn;
    //private ItemData itemData;

    void ShowButton(ItemData item)
    {
        EquipBtn.gameObject.SetActive(false);
        UnequipBtn.gameObject.SetActive(false);
        UseBtn.gameObject.SetActive(false);
        PlaceBtn.gameObject.SetActive(false);

        switch (item.itemType)
        {
            case ItemType.Equipable:
                if (!item.isEquiped)
                {
                    EquipBtn.gameObject.SetActive(true);
                    EquipBtn.onClick.RemoveAllListeners(); // 원래꺼 지우기
                    EquipBtn.onClick.AddListener(() => OnEquipButton(item));
                }
                else
                {
                    UnequipBtn.gameObject.SetActive(true);
                    UnequipBtn.onClick.RemoveAllListeners();
                    UnequipBtn.onClick.AddListener(() => OnUnequipButton(item)); 
                }
                break;
            case ItemType.Placeable:
                PlaceBtn.gameObject.SetActive(true);
                PlaceBtn.onClick.RemoveAllListeners();
                PlaceBtn.onClick.AddListener(() => OnPlaceBtn(item));
                break;
            case ItemType.Consumable:
                UseBtn.gameObject.SetActive(true);
                UseBtn.onClick.RemoveAllListeners();
                UseBtn.onClick.AddListener(() => OnUseBtn(item));
                break;
            default:
                break;
        }

        DropBtn.onClick.RemoveAllListeners();
        DropBtn.onClick.AddListener(() => OnDropBtn(item));
    }

    void OnEquipButton(ItemData item)
    {
        GameManager.Instance.player.playerEquip.Equip(item);
    }

    void OnUnequipButton(ItemData item)
    {
        GameManager.Instance.player.playerEquip.Unequip(item);
    }

    void OnUseBtn(ItemData item)
    {
        GameManager.Instance.player.condition.UseConsumableItem(item);
        Inventory.Instance.RemoveItem(item);
        UIManager.Instance.inventoryUI.UpdateUI();
    }

    void OnPlaceBtn(ItemData item)
    {
        GameManager.Instance.player.itemPlaceController.StartPlacing(item);
        Inventory.Instance.RemoveItem(item);
        UIManager.Instance.inventoryUI.UpdateUI();
    }

    void OnDropBtn(ItemData item)
    {
        if(item.isEquiped) // 장착중인 아이템 장착 해제 후 버리기.
        {
            GameManager.Instance.player.playerEquip.Unequip(item);
        }
        item.ToDropItem(GameManager.Instance.player.transform.position + Vector3.forward * 0.5f, Quaternion.identity);
        Inventory.Instance.RemoveItem(item);
        UIManager.Instance.inventoryUI.UpdateUI();
    }
}
