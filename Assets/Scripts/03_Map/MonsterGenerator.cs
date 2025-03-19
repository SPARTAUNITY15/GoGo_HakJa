using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    [SerializeField] Transform parent;//부모 오브젝트
    [SerializeField] GameObject[] prefabs;//생성할 오브젝트
    [SerializeField] List<GameObject> monsters;

    [Header("Raycast Setting")]
    [SerializeField] int destiny;//생성할 갯수

    [Space]

    [SerializeField] float distance = 100;//몬스터가 유지되는 거리
    //생성 범위
    [SerializeField] public float minHeight;
    [SerializeField] public float maxHeight;
    [SerializeField] public Vector2 xRange;
    [SerializeField] public Vector2 zRange;
    [SerializeField] float spawnRange;//스폰거리

    [Space]

    float currentTime;
    float createTime;
    [SerializeField] float createIntervel = 10;//생성 간격

    private void Awake()
    {
        monsters = new List<GameObject>();
    }

    private void Update()
    {
        currentTime = Time.time;
        if(currentTime - createTime > createIntervel)
        {
            Generate();
            createTime = currentTime;
        }
        for (int i = 0; i < monsters.Count; i++)
        {
            if(Vector3.Distance(GameManager.Instance.player.transform.position, monsters[i].transform.position) >= distance)//거리가 멀어지면 삭제
            {
                GameObject go = monsters[i];
                monsters.Remove(monsters[i]);
                Destroy(go.gameObject);
            }
        }
    }

    public void Generate()
    {
        int floorLayer = LayerMask.GetMask("Floor");

        for(int i = 0; i < destiny; i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            //랜덤한 위치 생성
            float sampleX = Random.Range(-spawnRange, spawnRange) + GameManager.Instance.player.transform.position.x;
            float sampleY = Random.Range(-spawnRange, spawnRange) + GameManager.Instance.player.transform.position.z;
            Vector3 ray = new Vector3(sampleX, maxHeight, sampleY);
            

            if(sampleX < xRange.x || sampleX > xRange.y || sampleY < zRange.x || sampleY > zRange.y)
            {
                continue;
            }
            if (!Physics.Raycast(ray, Vector3.down, out RaycastHit hit, maxHeight - minHeight + 1, floorLayer))
            {
                continue;
            }

            if (hit.point.y < minHeight)
            {
                continue;
            }

            GameObject go = Instantiate(prefab, parent);//오브젝트 생성
            monsters.Add(go);
            go.transform.position = hit.point;
        }
    }
}
