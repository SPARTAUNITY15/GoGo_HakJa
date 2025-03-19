using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Resources : MonoBehaviour, IImpactable
{
    //위치 제한
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
            if (!isFlag)//위치를 변경
            {
                RandomMove();
            }
            else//보이지 않게(프리펩안에 포함된 오브젝트)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void RandomMove()//위치 변경
    {
        int floorLayer = LayerMask.GetMask("Floor");

        while (true)
        {
            float sampleX = Random.Range(xRange.x, xRange.y);
            float sampleY = Random.Range(zRange.x, zRange.y);
            Vector3 ray = new Vector3(sampleX, maxHeight, sampleY);

            if (!Physics.Raycast(ray, Vector3.down, out RaycastHit hit, maxHeight - minHeight + 1, floorLayer))//땅에 닿지 않으면
            {
                continue;
            }
            if (hit.point.y < minHeight)
            {
                continue;
            }

            transform.position = hit.point;//위치 변경
            break;//반복문 종룍
        }
    }

}
