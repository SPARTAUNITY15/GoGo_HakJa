using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placed_CampFire : Placed_Item
{
    public override void SubscribeMethod()
    {
        UIManager.Instance.ToggleCursor();
        UIManager.Instance.ToggleUI("인벤토리");
        UIManager.Instance.inventoryUI.SetCraftMode(CraftMode.Campfire);
    }
}
