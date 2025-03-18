using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Resources : MonoBehaviour, IImpactable
{
    [SerializeField] public float minHeight;
    [SerializeField] public float maxHeight;
    [SerializeField] public Vector2 xRange;
    [SerializeField] public Vector2 zRange;

    public ItemData resourcePref;
    public float health;
    public bool isFlag = false;

    public void ReceiveImpact(float value)
    {
        health -= value;

        if (health <= 0)
        {
            resourcePref.ToDropItem(transform.position + Vector3.up, Quaternion.identity);
            if (!isFlag)
            {
                RandomMove();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void RandomMove()   
    {
        int floorLayer = LayerMask.GetMask("Floor");
        while (true)
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

            transform.position = hit.point;
            break;
        }
    }

}
