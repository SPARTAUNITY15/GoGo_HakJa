using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlacementGenerator : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] GameObject prefab;

    [Header("Raycast Setting")]
    [SerializeField] int destiny;

    [Space]

    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;
    [SerializeField] Vector2 xRange;
    [SerializeField] Vector2 zRange;

    [Header("Prefab Variation Setting")]
    [SerializeField] bool isOasis = false;
    [SerializeField, Range(0, 1)] float rotateTowardsNormal;
    [SerializeField] Vector2 rotationRange;
    [SerializeField] Vector3 minScale;
    [SerializeField] Vector3 maxScale;
    [SerializeField] float transformY = 0;

    private void Start()
    {
        if (!isOasis)
        {
            Generate();
        }
        else
        {
            GenerateOasis();
        }
    }

    public void Generate()
    {
        int floorLayer = LayerMask.GetMask("Floor");
        int i = 0;

        while (i < destiny)
        {
            float sampleX = Random.Range(xRange.x, xRange.y);
            float sampleY = Random.Range(zRange.x, zRange.y);
            Vector3 ray = new Vector3(sampleX, maxHeight, sampleY);

            if (!Physics.Raycast(ray, Vector3.down, out RaycastHit hit, maxHeight - minHeight + 1, floorLayer))
            {
                continue;
            }
            if (hit.point.y < minHeight)
            {
                continue;
            }

            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, transform);
            go.transform.parent = parent;
            go.transform.position = hit.point - new Vector3(0, transformY, 0);
            go.transform.Rotate(Vector3.up, Random.Range(rotationRange.x, rotationRange.y), Space.Self);
            go.transform.rotation = Quaternion.Lerp(transform.rotation,
                                                    transform.rotation * Quaternion.FromToRotation(go.transform.up, hit.normal),
                                                    rotateTowardsNormal);
            go.transform.localScale = new Vector3(
                Random.Range(minScale.x, maxScale.x),
                Random.Range(minScale.y, maxScale.y),
                Random.Range(minScale.z, maxScale.z)
                );
            i++;
        }
    }

    void GenerateOasis()
    {
        int floorLayer = LayerMask.GetMask("Floor");
        int i = 0;

        while (i < destiny)
        {
            float sampleX = xRange.x;
            float sampleY = zRange.x;
            Vector3 ray = new Vector3(sampleX, maxHeight, sampleY);

            if (!Physics.Raycast(ray, Vector3.down, out RaycastHit hit, maxHeight - minHeight + 1, floorLayer))
            {
                continue;
            }

            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, transform);
            go.transform.parent = parent;
            go.transform.position = hit.point - new Vector3(0, transformY, 0);
            go.transform.Rotate(Vector3.up, Random.Range(rotationRange.x, rotationRange.y), Space.Self);
            go.transform.rotation = Quaternion.Lerp(transform.rotation,
                                                    transform.rotation * Quaternion.FromToRotation(go.transform.up, hit.normal),
                                                    rotateTowardsNormal);
            go.transform.localScale = new Vector3(
                Random.Range(minScale.x, maxScale.x),
                Random.Range(minScale.y, maxScale.y),
                Random.Range(minScale.z, maxScale.z)
                );
            i++;
        }
    }
}
