using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour, IImpactable
{
    public ItemData resourcePref;
    public void ReceiveImpact(float value)
    {
        resourcePref.ToDropItem();// �ڿ� ����
    }
}
