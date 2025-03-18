using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlacementGenerator : MonoBehaviour
{
    [SerializeField] Transform parent;//부모 오브젝트
    [SerializeField] GameObject prefab;//생성할 오브젝트

    [Header("Raycast Setting")]
    [SerializeField] int destiny;//생성할 갯수

    [Space]
    //생성 범위
    [SerializeField] public float minHeight;
    [SerializeField] public float maxHeight;
    [SerializeField] public Vector2 xRange;
    [SerializeField] public Vector2 zRange;

    [Header("Prefab Variation Setting")]
    [SerializeField] bool isOasis = false;
    [SerializeField, Range(0, 1)] float rotateTowardsNormal;
    [SerializeField] Vector2 rotationRange;
    [SerializeField] Vector3 minScale;
    [SerializeField] Vector3 maxScale;
    [SerializeField] public float transformY = 0;

    private void Start()
    {
        if (!isOasis)//오아시스가 아닐때
        {
            Generate();
        }
        else//오아시스 일때
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
            //랜덤한 위치 생성
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

            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, transform);//오브젝트 생성
            //오브젝트 값 세팅
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

    void GenerateOasis()//오아시스 생성
    {
        int floorLayer = LayerMask.GetMask("Floor");
        int i = 0;

        while (i < destiny)
        {
            //정해진 위치에 생성
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
