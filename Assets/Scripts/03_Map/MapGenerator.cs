using System.Threading.Tasks;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("TileMap")]
    [SerializeField] private Terrain terrain;

    [Header("Map")]
    [SerializeField] private float mapScale;
    [SerializeField] private int mapSize;
    [SerializeField] private int depth;

    [SerializeField] private int octaves;
    private float seed;

    private async void Start()
    {
        seed = Random.Range(0, 10000f);
        var noiseArr = await Task.Run(GenerateNoise);
        SetTerrain(noiseArr);
    }

    private void SetTerrain(float[,] noiseArr)
    {
        terrain.terrainData.size = new Vector3 (mapSize, depth, mapSize);
        terrain.terrainData.SetHeights(0, 0, noiseArr);
    }

    private float[,] GenerateNoise()
    {
        float[,] noiseArr = new float[mapSize, mapSize];
        float min = float.MaxValue;
        float max = float.MinValue;

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                float lacunarity = 2.0f;
                float gain = 0.5f;
                float amplitude = 0.5f;
                float frequency = 1f;

                for (int i = 0; i < octaves; i++)
                {

                    noiseArr[x, y] += amplitude * (Mathf.PerlinNoise(
                        seed + (x * mapScale * frequency),
                        seed + (y * mapScale * frequency)) * 2 - 1);

                    frequency *= lacunarity;
                    amplitude *= gain;

                }
                if (noiseArr[x, y] < min)
                {
                    min = noiseArr[x, y];
                }
                else if (noiseArr[x, y] > max)
                {
                    max = noiseArr[x, y];
                }
            }
        }

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                noiseArr[x, y] = Mathf.InverseLerp(min, max, noiseArr[x, y]);
            }
        }

        return noiseArr;
    }
}
