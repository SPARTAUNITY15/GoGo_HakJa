using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placed_Tent : Placed_Item
{
    private void Start()
    {
        //�������� ������Ʈ ����
        GameObject safezone = new GameObject("SafeZone");
        safezone.transform.SetParent(transform,false);
        safezone.AddComponent<SafeZone>();
    }

    public override void SubscribeMethod()
    {
        Debug.Log("�̱���");
    }
}
