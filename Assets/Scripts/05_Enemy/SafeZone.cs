using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public bool isPlayerInside = false; // �÷��̾ �ȿ� �ִ��� ����

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

