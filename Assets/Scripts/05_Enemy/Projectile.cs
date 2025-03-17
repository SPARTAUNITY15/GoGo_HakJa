using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;   // ����ü �ӵ�
    public int damage = 10;     // ������
    public float lifetime = 3f; // ���� �ð��� ������ ����

    private Vector3 targetDirection;

    public void SetTarget(Vector3 direction)
    {
        targetDirection = direction.normalized;
    }

    void Update()
    {
        transform.position += targetDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾�� ������ �������� ��
        {
            ///������ �ִ� �޼ҵ� ���� �߰� 
            Destroy(gameObject); // ����ü ����
        }
    }
}
