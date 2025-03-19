using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 7f;
    public float gravity = 9.8f; // 중력 가속도
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
            Debug.LogError("Projectile에 Rigidbody가 없습니다! Rigidbody를 추가해주세요.");
        }
    }

    private void Start()
    {
        // 투사체를 포물선 궤도로 발사
        LaunchProjectile();

        Destroy(gameObject, lifetime);
    }

    private void LaunchProjectile()
    {
        if (rb != null)
        {
            // 발사 방향과 힘을 설정 (앞으로 + 위로)
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
