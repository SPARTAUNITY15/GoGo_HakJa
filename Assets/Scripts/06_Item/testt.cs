using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testt : MonoBehaviour
{
    public ItemData itemdata;

    void Start()
    {
        itemdata.ToDropItem();
        itemdata.ToPlacedItem(Vector3.zero, Quaternion.Euler(Vector3.zero));
    }
}
