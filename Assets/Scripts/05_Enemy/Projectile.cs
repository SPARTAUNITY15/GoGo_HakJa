using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;   // 투사체 속도
    public int damage = 10;     // 데미지
    public float lifetime = 3f; // 일정 시간이 지나면 삭제

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
        if (other.CompareTag("Player")) // 플레이어에게 맞으면 데미지를 줌
        {
            ///데미지 주는 메소드 추후 추가 
            Destroy(gameObject); // 투사체 삭제
        }
    }
}
