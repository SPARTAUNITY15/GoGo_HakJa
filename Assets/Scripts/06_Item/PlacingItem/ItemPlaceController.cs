using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 설치 트리거. "GameManager.Instance.player.itemPlaceController.StartPlacing(itemData)"
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
        // 청사진 생성
        Preview_Item ghost = item2Place.ToPreviewItem(Vector3.zero, Quaternion.identity).GetComponent<Preview_Item>(); // 임시로 placed item으로 생성해뒀는데 좀 더 생각해봐야할듯.

        // 생성 오브젝트의 init 실행.
        ghost.Init(item2Place, this, floatingHeight);

        // 반환
        return ghost;
    }

    public void QuitPlacing()
    {
        isPlacing = false;
        Destroy(curPlacingItem.gameObject);
    }
}
