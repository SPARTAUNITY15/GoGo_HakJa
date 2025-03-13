using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;  // ���̷��� ������
    public int enemyCount = 2;      // ������ �� ����
    public Vector3 spawnArea;       // ���� ����

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(-spawnArea.x, spawnArea.x),
                0,
                Random.Range(-spawnArea.z, spawnArea.z)
            );

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }
}

