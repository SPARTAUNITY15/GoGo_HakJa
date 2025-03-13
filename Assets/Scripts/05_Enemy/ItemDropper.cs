using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public GameObject dropItemPrefab;
    public float dropForce = 2f; // �������� ��¦ Ƣ����� ��

    public void DropItem()
    {
        if (dropItemPrefab != null)
        {
            // ���� ��ġ���� ������ ����
            GameObject droppedItem = Instantiate(dropItemPrefab, transform.position, Quaternion.identity);

            // Rigidbody�� �ִٸ� ���� ���ؼ� �ڿ������� �������� �����
            Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 randomForce = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)) * dropForce;
                rb.AddForce(randomForce, ForceMode.Impulse);
            }
        }
    }
}

