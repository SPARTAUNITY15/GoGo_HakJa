using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public GameObject dropItemPrefab;
    public float dropForce = 2f; // 아이템이 살짝 튀어나가는 힘

    public void DropItem()
    {
        if (dropItemPrefab != null)
        {
            // 현재 위치에서 아이템 생성
            GameObject droppedItem = Instantiate(dropItemPrefab, transform.position, Quaternion.identity);

            // Rigidbody가 있다면 힘을 가해서 자연스럽게 떨어지게 만들기
            Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 randomForce = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)) * dropForce;
                rb.AddForce(randomForce, ForceMode.Impulse);
            }
        }
    }
}

