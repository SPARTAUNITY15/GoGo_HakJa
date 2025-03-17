using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_Player : MonoBehaviour
{
    public ItemData item2Place;
    public ItemPlaceController placeController;

    int i = 100;
    private void Update()
    {
        i--;
        placeController = GetComponent<ItemPlaceController>();
        if (i == 0)
            placeController.StartPlacing(item2Place);
    }
}
