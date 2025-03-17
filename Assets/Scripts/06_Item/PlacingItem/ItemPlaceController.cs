using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ��ġ Ʈ����. "GameManager.Instance.player.itemPlaceController.StartPlacing(itemData)"
public class ItemPlaceController : MonoBehaviour
{
    bool isPlacing;
    Preview_Item curPlacingItem;
    [SerializeField] private float floatingHeight = 1;

    public void StartPlacing(ItemData item2Place)
    {
        if (!isPlacing)
        {
            isPlacing = true;
            curPlacingItem = MakePreviewItem(item2Place);
        }
    }

    private Preview_Item MakePreviewItem(ItemData item2Place)
    {
        // û���� ����
        Preview_Item ghost = item2Place.ToPreviewItem(Vector3.zero, Quaternion.identity).GetComponent<Preview_Item>(); // �ӽ÷� placed item���� �����ص״µ� �� �� �����غ����ҵ�.

        // ���� ������Ʈ�� init ����.
        ghost.Init(item2Place, this, floatingHeight);

        // ��ȯ
        return ghost;
    }

    public void QuitPlacing()
    {
        isPlacing = false;
        Destroy(curPlacingItem.gameObject);
    }
}
