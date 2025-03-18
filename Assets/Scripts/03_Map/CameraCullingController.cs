using UnityEngine;

public class CameraCullingController : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask layerMask;
    public ItemData item;
    public bool condition = false;

    private void Update()
    {
        if(GameManager.Instance.player.playerEquip.equippedItem.itemData == item || condition)
        {
            mainCamera.cullingMask |= layerMask;
        }
        else
        {
            mainCamera.cullingMask &= ~layerMask;
        }
    }
}
