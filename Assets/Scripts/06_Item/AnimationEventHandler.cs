using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    Equip_Item equipItem;

    private void Start()
    {
        equipItem = GetComponentInParent<Equip_Item>();
    }
    public void OnHit()
    {
        equipItem.PerformEquipInteraction();
    }
}
