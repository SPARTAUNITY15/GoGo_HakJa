using UnityEngine;

public class CameraCullingController : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask layerMask;
    public ItemData item;
    public bool condition = false;

    private void Update()
    {
        if (condition || (GameManager.Instance.player.playerEquip.isEquipping && GameManager.Instance.player.playerEquip.equippedItem.itemData == item))
        {
            mainCamera.cullingMask |= layerMask;//cullingMask에 추가
        }
        else
        {
            mainCamera.cullingMask &= ~layerMask;//cullingMask에서 제거
        }
    }
}
