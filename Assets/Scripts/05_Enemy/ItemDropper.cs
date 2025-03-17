using UnityEngine;
using System.Collections;

public class ItemDropper : MonoBehaviour
{
    public GameObject dropItemPrefab;
    public ItemData itemData; 
    public float dropForce = 2f; // 아이템이 살짝 튀어나가는 힘

    //코루틴을 이용해서 아이템 드롭 시간 지연시키기
    public void DropItemWithDelay(float delay)
    {
        StartCoroutine(DropItemCoroutine(delay));
    }

    private IEnumerator DropItemCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay); 
        DropItem();
    }
    public void DropItem()
    {
        if (dropItemPrefab != null)
        {
            // 현재 위치에서 아이템 생성
            GameObject droppedItem = Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
            //GameObject go = itemData.ToDropItem(transform.position, Quaternion.identity);
            
            Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
            //Rigidbody rb = go.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 randomForce = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)) * dropForce;
                rb.AddForce(randomForce, ForceMode.Impulse);
            }
            else if (rb == null)
            {
                Debug.Log("아이템프리팹 안에 RigidBody가 없습니다! (생성되는데에는 문제가 없습니다.)");
            }
        }
    }
}

