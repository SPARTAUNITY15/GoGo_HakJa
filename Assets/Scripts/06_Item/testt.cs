using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testt : MonoBehaviour
{
    public ItemData itemdata;
    public Player player;
    public Camera equipCamera;
    private Equip_Item equip;

    void Start()
    {
        equip = Instantiate(itemdata.equipPref, equipCamera.transform).GetComponent<Equip_Item>();
        itemdata.ToDropItem(player.transform);

        //StartCoroutine(coTest());
    }

    //IEnumerator coTest()
    //{
    //    yield return new WaitForSeconds(1);

    //    test();
    //}

    private void test()
    {
        equip.StartEquipInteraction();
    }

    int i = 100;
    private void Update()
    {
        i--;
        test();

    }
}
