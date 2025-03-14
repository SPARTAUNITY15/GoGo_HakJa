using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class PlacementGenerator : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    [Header("Raycast Setting")]
    [SerializeField] int destiny;

    [Space]

    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;
    [SerializeField] Vector2 xRange;
    [SerializeField] Vector2 zRange;

    [Header("Prefab Variation Setting")]
    [SerializeField, Range(0, 1)] float rotateTowardsNormal;
    [SerializeField] Vector2 rotationRange;
    [SerializeField] Vector3 minScale;
    [SerializeField] Vector3 maxScale;

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        Clear();

        for (int i = 0; i < destiny; i++)
        {
            float sampleX = Random.Range(xRange.x, xRange.y);
            float sampleY = Random.Range(zRange.x, zRange.y);
            Vector3 ray = new Vector3(sampleX, maxHeight, sampleY);

            if (!Physics.Raycast(ray, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                continue;
            }
            if (hit.point.y < minHeight)
            {
                continue;
            }

            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, transform);
            go.transform.position = hit.point;
            go.transform.Rotate(Vector3.up, Random.Range(rotationRange.x, rotationRange.y), Space.Self);
            go.transform.rotation = Quaternion.Lerp(transform.rotation,
                                                    transform.rotation * Quaternion.FromToRotation(go.transform.up, hit.normal),
                                                    rotateTowardsNormal);
            go.transform.localScale = new Vector3(
                Random.Range(minScale.x, maxScale.x),
                Random.Range(minScale.y, maxScale.y),
                Random.Range(minScale.z, maxScale.z)
                );
        }
    }

    public void Clear()
    {
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
