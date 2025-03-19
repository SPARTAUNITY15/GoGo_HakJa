using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 7f;
    public float gravity = 9.8f; // �߷� ���ӵ�
    public int damage = 10;
    public float lifetime = 5f;

    private Rigidbody rb;
    private Vector3 targetDirection;

    public SpiderAI spiderAI;

    private Renderer projectileRenderer;
    private Color originalColor;

    public void SetTarget(Vector3 direction)
    {
        targetDirection = direction.normalized;
    }

    private void Awake()
    {
        originalColor = Color.green;
        projectileRenderer = GetComponent<Renderer>();
        projectileRenderer.material.color = originalColor;

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Projectile�� Rigidbody�� �����ϴ�! Rigidbody�� �߰����ּ���.");
        }
    }

    private void Start()
    {
        // ����ü�� ������ �˵��� �߻�
        LaunchProjectile();

        Destroy(gameObject, lifetime);
    }

    private void LaunchProjectile()
    {
        if (rb != null)
        {
            // �߻� ����� ���� ���� (������ + ����)
            Vector3 launchVelocity = targetDirection * speed + Vector3.up * (speed / 2);
            rb.velocity = launchVelocity;
            rb.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spiderAI.DamagePlayer();
            Destroy(gameObject);
        }
    }
}
