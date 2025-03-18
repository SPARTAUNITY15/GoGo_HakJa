using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class FlattenRegion //��źȭ�� ����
{
    public int startX;
    public int endX;
    public int startZ;
    public int endZ;
    public float constantHeight = 0.5f; // ��źȭ�� ����
    public int blendWidth = 20;         // ��迡�� ������ ��
}

public class MapGenerator : MonoBehaviour
{
    [Header("TileMap")]
    [SerializeField] private Terrain terrain;

    [Header("Map")]
    public int mapSize; //�� ũ��
    public float depth; //�� ����
    public float scale; //Ȯ�� ���� Ŭ���� �ε巯��
    public int octaves; //��ø��
    [Range(0, 1)] public float persistance; //���Ӽ� Ŭ���� �� ����
    public float lacunarity;//���ļ� ������ ���� ���� �ε巯��
    public int seed;//�õ�
    public Vector2 offset;//�������� ���� ��
    public float offsetRanX;
    public float offsetRanZ;
    public float chunkX;
    public float chunkZ;
    float[,] noiseMap;

    [Header("Flatten Regions")]
    public FlattenRegion[] flattenRegions;

    private void Awake()
    {
        seed = UnityEngine.Random.Range(0, 100000); //������ �õ�
        var noiseArr = CreateNoise();//������ ����
        SetTerrain(noiseArr);//���� ����
    }

    private void SetTerrain(float[,] noiseArr)//���� ����
    {
        terrain.terrainData.size = new Vector3 (mapSize, depth, mapSize);
        terrain.terrainData.SetHeights(0, 0, noiseArr);
    }

    float[,] CreateNoise()//������ ����
    {
        noiseMap = new float[mapSize, mapSize];
        float min = float.MaxValue;
        float max = float.MinValue;
        System.Random prng = new System.Random(seed);//������ �õ�� ������ ���� ����
        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            offsetRanX = prng.Next(-100000, 100000) + offset.x;
            offsetRanZ = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetRanX, offsetRanZ);
        }

        if (scale <= 0)//scale�� �ּڰ�
        {
            scale = 0.0001f;
        }

        float halfXSize = mapSize / 2f;
        float halfZSize = mapSize / 2f;

        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                float amplitude = 1;//����
                float frequency = 1;//���ļ�
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)//��ø
                {
                    float sampleX = ((x - halfXSize) + chunkX * mapSize) / scale * frequency + octaveOffsets[i].x;
                    float sampleZ = ((z - halfZSize) + chunkZ * mapSize) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > max)//�ִ� ����
                {
                    max = noiseHeight;
                }
                if (noiseHeight < min)//�ּڰ� ����
                {
                    min = noiseHeight;
                }
                noiseMap[x, z] = noiseHeight;
            }
        }

        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                float finalBlend = 0f;
                float targetHeight = noiseMap[x, z]; // �⺻��

                foreach (var region in flattenRegions)
                {
                    // x�࿡ ���� ���� ���� ���
                    float blendX = 0f;
                    if (x >= region.startX - region.blendWidth && x < region.startX)
                    {
                        blendX = Mathf.SmoothStep(0f, 1f, (x - (region.startX - region.blendWidth)) / (float)region.blendWidth);
                    }
                    else if (x >= region.startX && x <= region.endX)
                    {
                        blendX = 1f;
                    }
                    else if (x > region.endX && x <= region.endX + region.blendWidth)
                    {
                        blendX = Mathf.SmoothStep(1f, 0f, (x - region.endX) / (float)region.blendWidth);
                    }

                    // z�࿡ ���� ���� ���� ���
                    float blendZ = 0f;
                    if (z >= region.startZ - region.blendWidth && z < region.startZ)
                    {
                        blendZ = Mathf.SmoothStep(0f, 1f, (z - (region.startZ - region.blendWidth)) / (float)region.blendWidth);
                    }
                    else if (z >= region.startZ && z <= region.endZ)
                    {
                        blendZ = 1f;
                    }
                    else if (z > region.endZ && z <= region.endZ + region.blendWidth)
                    {
                        blendZ = Mathf.SmoothStep(1f, 0f, (z - region.endZ) / (float)region.blendWidth);
                    }

                    // ���� ���� ���ڴ� �� ���� �ּҰ� (�� ���� ��� ��谡 ����Ǿ�� ��)
                    float regionBlend = Mathf.Min(blendX, blendZ);

                    // ���� ������ ��ĥ ���, ���� ���� �� ū ������ �켱��
                    if (regionBlend > finalBlend)
                    {
                        finalBlend = regionBlend;
                        targetHeight = region.constantHeight;
                    }
                }

                // �ش� ��ǥ�� ������� ��źȭ ���̸� �ε巴�� ȥ��
                noiseMap[x, z] = Mathf.Lerp(noiseMap[x, z], targetHeight, finalBlend);
            }
        }
        return noiseMap;
    }
}
