using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;  // 스켈레톤 프리팹
    public int enemyCount = 2;      // 스폰할 적 개수
    public Vector3 spawnArea;       // 스폰 범위

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

