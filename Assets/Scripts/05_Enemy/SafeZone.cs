using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public bool isPlayerInside = false; // 플레이어가 안에 있는지 여부

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

