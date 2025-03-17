using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testt : MonoBehaviour
{

    /* 테스트: 무기 장착 / 애니메이션 실행 / 타격 판정 테스트
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
    */

    public ItemData itemData;

    private void Start()
    {
        itemData.ToDropItem();
    }

}
