using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
struct Reward//보상 아이템과 확률
{
    [SerializeField] ItemData item;//보상 아이템
    public ItemData Item { get { return item; } }

    [SerializeField] int probability;//확률
    public int Probability { get { return probability; } }
}

public class ResourceInRandom: MonoBehaviour, IInteractable
{
    [SerializeField]Reward[] rewards;//보상 목록
    public ItemData resourcePref;//선택된 보상
    public float health;//체력

    public void GetPromptInfo()
    {
        health--;

        if (health <= 0)//보상생성
        {
            resourcePref.ToDropItem(transform.position + Vector3.up * 2, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void SubscribeMethod()
    {
        Debug.Log("item interactive");
    }

    void Start()
    {
        if (rewards == null || rewards.Length == 0)//null 확인
        {
            Debug.Log("No rewards available!");
            return;
        }

        int totalProbability = 0;//총 확률
        foreach (Reward reward in rewards)
        {
            totalProbability += reward.Probability;
        }

        int randomValue = Random.Range(0, totalProbability);

        foreach (Reward reward in rewards)//보상 선택
        {
            if (randomValue < reward.Probability)
            {
                resourcePref = reward.Item;
                break;
            }
            randomValue -= reward.Probability;
        }
    }

    public string GetPromptName()
    {
        return resourcePref.name;
    }

    public string GetPromptDesc()
    {
        return resourcePref.item_description + "\n [G] 상호작용";
    }
}
