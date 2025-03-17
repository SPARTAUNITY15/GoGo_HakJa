using UnityEngine;
using System.Collections;

public class ItemDropper : MonoBehaviour
{
    public GameObject dropItemPrefab;
    public ItemData itemData;
    public float dropForce = 3f; // �������� ��¦ Ƣ����� ��
    public GameObject dropParticleEffectPrefab;

    private bool hasDropped = false; // �� ���� ����ϱ� ���� �÷���

    // �ڷ�ƾ�� �̿��ؼ� ������ ��� �ð� ������Ű��
    public void DropItemWithDelay(float delay)
    {
        if (!hasDropped)
        {
            hasDropped = true;
            StartCoroutine(DropItemCoroutine(delay));
        }
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

            if (dropParticleEffectPrefab != null)
            {
                GameObject particleEffect = Instantiate(dropParticleEffectPrefab, transform.position, Quaternion.identity);
                ParticleSystem ps = particleEffect.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    Destroy(particleEffect, ps.main.duration + ps.main.startLifetime.constantMax);
                }
                else
                {
                    Destroy(particleEffect, 3f);
                }
            }

            Rigidbody rb = go.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 randomForce = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)) * dropForce;
                rb.AddForce(randomForce, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("������������ �ȿ� Rigidbody�� �����ϴ�! (�����Ǵµ����� ������ �����ϴ�.)");
            }
        }
    }
}
