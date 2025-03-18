using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public float lifetime;

    private Vector3 targetDirection;

    private Renderer projectileRenderer;
    private Color originalColor;

    public SpiderAI spiderAI;
    public void SetTarget(Vector3 direction)
    {
        targetDirection = direction.normalized;
    }

    private void Awake()
    {
        originalColor = Color.green;
        projectileRenderer = GetComponent<Renderer>();
        projectileRenderer.material.color = originalColor;
    }
    private void Start()
    {
        //����ü �� �¾��� ��� lifetime ���� ����
        Destroy(gameObject, lifetime);
    }
    void Update()
    {
        transform.position += targetDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spiderAI.DamagePlayer();
            Destroy(gameObject); // ����ü ����
        }
    }
}
