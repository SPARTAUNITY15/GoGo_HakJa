using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SafeZone : MonoBehaviour
{
    public bool isPlayerInside; // �÷��̾ �ȿ� �ִ��� ����
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
            Debug.Log("�÷��̾� ��ã�� �Ф�");
        }

        Debug.Log("1");
        
        float distance = (player.transform.position - transform.position).magnitude;
        if (distance <= colli.radius)
        {
            Debug.Log("�ʱ�ȭ �� Player�� �̹� �������� �ȿ� ����");
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("Player ����");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("Player ����");
        }
    }
}

