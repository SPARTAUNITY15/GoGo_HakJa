using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
struct Reward
{
    [SerializeField] ItemData item;
    public ItemData Item { get { return item; } }
    [SerializeField] int probability;
    public int Probability { get { return probability; } }
}

public class ResourceInRandom: MonoBehaviour, IInteractable
{
    [SerializeField]Reward[] rewards;
    public ItemData resourcePref;
    public float health;

    public void GetPromptInfo()
    {
        health--;

        if (health <= 0)
        {
            resourcePref.ToDropItem(transform.position + Vector3.up, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void SubscribeMethod()
    {
        Debug.Log("item interactive");
    }

    void Start()
    {
        if (rewards == null || rewards.Length == 0)
        {
            Debug.Log("No rewards available!");
            return;
        }

        int totalProbability = 0;
        foreach (Reward reward in rewards)
        {
            totalProbability += reward.Probability;
        }

        int randomValue = Random.Range(0, totalProbability);

        foreach (Reward reward in rewards)
        {
            if (randomValue < reward.Probability)
            {
                resourcePref = reward.Item;
                break;
            }
            randomValue -= reward.Probability;
        }
    }
}
