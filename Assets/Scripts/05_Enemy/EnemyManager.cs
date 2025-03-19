using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;  
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;

    public int enemyCount; 
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
            Instantiate(enemyPrefab2, spawnPos, Quaternion.identity);
            Instantiate(enemyPrefab3, spawnPos, Quaternion.identity);
        }
    }
}

