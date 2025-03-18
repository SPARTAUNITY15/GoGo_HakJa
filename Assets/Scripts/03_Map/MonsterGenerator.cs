using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    [SerializeField] Transform parent;//�θ� ������Ʈ
    [SerializeField] GameObject[] prefabs;//������ ������Ʈ
    [SerializeField] List<GameObject> monsters;

    [Header("Raycast Setting")]
    [SerializeField] int destiny;//������ ����

    [Space]

    [SerializeField] float distance = 100;//���Ͱ� �����Ǵ� �Ÿ�
    //���� ����
    [SerializeField] public float minHeight;
    [SerializeField] public float maxHeight;
    [SerializeField] public Vector2 xRange;
    [SerializeField] public Vector2 zRange;
    [SerializeField] float spawnRange;//�����Ÿ�

    [Space]

    float currentTime;
    float createTime;
    [SerializeField] float createIntervel = 10;//���� ����

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
            if(Vector3.Distance(GameManager.Instance.player.transform.position, monsters[i].transform.position) >= distance)//�Ÿ��� �־����� ����
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
            //������ ��ġ ����
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

            GameObject go = Instantiate(prefab, parent);//������Ʈ ����
            monsters.Add(go);
            go.transform.position = hit.point;
        }
    }
}
