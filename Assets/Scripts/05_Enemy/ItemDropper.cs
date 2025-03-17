using UnityEngine;
using System.Collections;

public class ItemDropper : MonoBehaviour
{
    public GameObject dropItemPrefab;
    public ItemData itemData; 
    public float dropForce = 3f; // �������� ��¦ Ƣ����� ��

    //�ڷ�ƾ�� �̿��ؼ� ������ ��� �ð� ������Ű��
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
        if (itemData != null)
        {
            GameObject go = itemData.ToDropItem(transform.position, Quaternion.identity);
            
            Rigidbody rb = go.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 randomForce = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)) * dropForce;
                rb.AddForce(randomForce, ForceMode.Impulse);
            }
            else if (rb == null)
            {
                Debug.Log("������������ �ȿ� RigidBody�� �����ϴ�! (�����Ǵµ����� ������ �����ϴ�.)");
            }
        }
    }
}

