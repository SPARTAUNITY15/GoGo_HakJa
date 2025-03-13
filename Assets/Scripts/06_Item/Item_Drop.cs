using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Item_Drop : MonoBehaviour //IInteractable
{
    public ItemData itemData;
    public GameObject rootObject; // 필요없을듯.

    //public void SubscribeMethod()
    //{
    //    GameManager.Instance.player.controller.interactAction = GameManager.Instance.player.inventory.AddItem;
    //}

    //public GameObject GetRoot()
    //{
    //    if (rootObject != null)
    //        return rootObject; // 필요 없을듯
    //    return gameObject;
    //}

    //public Wrapping GetNeedThing()
    //{
    //    var wrapped = new Wrapping();
    //    wrapped._itemData = itemData;
    //    return wrapped;
    //}

    public (string, string) GetPromptInfo()
    {
        return (itemData.item_name, itemData.item_description);
    }

}
