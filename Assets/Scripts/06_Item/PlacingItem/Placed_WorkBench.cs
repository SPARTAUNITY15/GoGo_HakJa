using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placed_WorkBench : Placed_Item
{
    public override void SubscribeMethod()
    {
        UIManager.Instance.ToggleCursor();
        UIManager.Instance.ToggleUI("�κ��丮");
        UIManager.Instance.inventoryUI.SetCraftMode(CraftMode.WorkBench);
    }
}
