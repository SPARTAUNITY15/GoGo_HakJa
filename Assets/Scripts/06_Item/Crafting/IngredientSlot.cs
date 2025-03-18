using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSlot : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI TMP;

    public void Init(itemWithCount itemWithCount)
    {
        image.sprite = itemWithCount.stuff.Icon;
        SetText(itemWithCount.stuff_count);
    }

    private void SetText(int count)
    {
        if(count > 0)
        {
            image.enabled = true;
            TMP.enabled = true;
            TMP.text = count.ToString();
        }
        else
        {
            image.enabled = false;
            TMP.enabled = false;
        }
    }
}
