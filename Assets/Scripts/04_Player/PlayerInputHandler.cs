using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public void OnInventoryInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            UIManager.Instance.ToggleUI("�κ��丮");
            UIManager.Instance.inventoryUI.SetCraftMode(CraftMode.Inventory);
        }
    }
}
