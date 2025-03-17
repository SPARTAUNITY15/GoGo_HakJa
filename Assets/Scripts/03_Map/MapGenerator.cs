using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MapGenerator : MonoBehaviour
{
    [Header("TileMap")]
    [SerializeField] private Terrain terrain;

    [Header("Map")]
    public int mapSize;
    public float depth;
    public float scale;
    public int octaves;
    [Range(0, 1)] public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    public float offsetRanX;
    public float offsetRanZ;
    public float chunkX;
    public float chunkZ;
    float[,] noiseMap;

    [Header("Tree")]
    public GameObject prefab;
    [Range(0, 1)] public float density;

    private async void Awake()
    {
        var noiseArr = await Task.Run(CreateNoise);
        SetTerrain(noiseArr);
        //PlaceObjects();
    }

    private void SetTerrain(float[,] noiseArr)
    {
        terrain.terrainData.size = new Vector3 (mapSize, depth, mapSize);
        terrain.terrainData.SetHeights(0, 0, noiseArr);
    }

    float[,] CreateNoise()
    {
        noiseMap = new float[mapSize, mapSize];
        float min = float.MaxValue;
        float max = float.MinValue;
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            offsetRanX = prng.Next(-100000, 100000) + offset.x;
            offsetRanZ = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetRanX, offsetRanZ);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float halfXSize = mapSize / 2f;
        float halfZSize = mapSize / 2f;

        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = ((x - halfXSize) + chunkX * mapSize) / scale * frequency + octaveOffsets[i].x;
                    float sampleZ = ((z - halfZSize) + chunkZ * mapSize) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > max)
                {
                    max = noiseHeight;
                }
                if (noiseHeight < min)
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
                noiseMap[x, z] = Mathf.InverseLerp(min, max, noiseMap[x, z]);
            }
        }
        return noiseMap;
    }

    //public void PlaceObjects()
    //{
    //    Transform parent = new GameObject("PlacedObjects").transform;

    //    for (int x = 10; x < mapSize - 10; x++)
    //    {
    //        for (int z = 10; z < mapSize - 10; z++)
    //        {
    //            float noiseValue = Mathf.PerlinNoise((x + seed) / 5f, (z + seed) / 5);

    //            if (noiseValue > 1 - density)
    //            {
    //                float y = terrain.terrainData.GetInterpolatedHeight(x / (float)terrain.terrainData.size.x,
    //                                                                    z / (float)terrain.terrainData.size.y);
    //                Vector3 spawnPos = new Vector3(x, y, z);

    //                GameObject go = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
    //                go.transform.SetParent(parent);
    //            }
    //        }

    //    }
    //}
}
