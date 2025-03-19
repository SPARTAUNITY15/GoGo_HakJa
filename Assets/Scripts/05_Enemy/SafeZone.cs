using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SafeZone : MonoBehaviour
{
    public bool isPlayerInside; // 플레이어가 안에 있는지 여부
    SphereCollider colli;
    NavMeshObstacle obstacle;
    public float safezoneRadius = 7;
    private void Start()
    {
        colli = transform.gameObject.AddComponent<SphereCollider>();
        colli.radius = safezoneRadius;
        colli.isTrigger = true;

        obstacle = transform.AddComponent<NavMeshObstacle>();   

        Debug.Log(colli.radius);
        Debug.Log("2");
        isPlayerInside = CheckPlayerInSafeZone();
    }

    private bool CheckPlayerInSafeZone()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player == null)
        {
            Debug.Log("플레이어 못찾음 ㅠㅠ");
        }

        Debug.Log("1");
        
        float distance = (player.transform.position - transform.position).magnitude;
        if (distance <= colli.radius)
        {
            Debug.Log("초기화 시 Player가 이미 세이프존 안에 있음");
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("Player 들어옴");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("Player 나감");
        }
    }
}

