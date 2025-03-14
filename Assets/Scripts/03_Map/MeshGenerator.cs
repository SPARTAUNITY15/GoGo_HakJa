using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    [Header("Map")]
    public int xSize;
    public int zSize;
    public float height;
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

    public Gradient gradient;

    [SerializeField] float minTerrainHeight = -1.4f;
    [SerializeField] float maxTerrainHeight = 1.4f;

    float[,] noiseMap;
    public bool autoUpdate;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
    }

    public void CreateShape()
    {
        if (scale <= 0)
        {
            scale = 0.0001f;
        }
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        noiseMap = CreateNoise();

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x, noiseMap[x, z] * height, z);
                i++;
            }
        }
        SetTriangle();

        SetColor();
    }

    public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    void SetColor()
    {
        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                colors[i] = gradient.Evaluate(noiseMap[x, z]);
                i++;
            }
        }
    }

    void SetTriangle()
    {
        triangles = new int[xSize * zSize * 6];
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                int vert = z * (xSize + 1) + x;

                triangles[tris + 0] = vert;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;

                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                tris += 6;
            }
        }
    }

    float[,] CreateNoise()
    {
        noiseMap = new float[xSize + 1, zSize + 1];

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

        float halfXSize = xSize / 2f;
        float halfZSize = zSize / 2f;

        for (int x = 0; x <= xSize; x++)
        {
            for (int z = 0; z <= zSize; z++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = ((x - halfXSize) + chunkX * xSize) / scale * frequency + octaveOffsets[i].x;
                    float sampleZ = ((z - halfZSize) + chunkZ * zSize) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxTerrainHeight)
                {
                    //maxTerrainHeight = noiseHeight;
                    noiseHeight = maxTerrainHeight;
                }
                if (noiseHeight < minTerrainHeight)
                {
                    //minTerrainHeight = noiseHeight;
                    noiseHeight = minTerrainHeight;
                }
                noiseMap[x, z] = noiseHeight;
            }
        }

        for (int x = 0; x <= xSize; x++)
        {
            for (int z = 0; z <= zSize; z++)
            {
                noiseMap[x, z] = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, noiseMap[x, z]);
            }
        }
        return noiseMap;
    }

    void OnValidate()
    {
        if (xSize < 1)
        {
            xSize = 1;
        }
        if (zSize < 1)
        {
            zSize = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    // vertices 배열과 triangles 배열이 생성되어 있을 때만 실행
    //    if (vertices == null)
    //        return;

    //    // 정점 위치를 검은색 구체로 표시 (월드 좌표로 변환)
    //    Gizmos.color = Color.black;
    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        Gizmos.DrawSphere(transform.TransformPoint(vertices[i]), 0.1f);
    //    }
    //}

}
