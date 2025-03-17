using UnityEngine;

public class Resources : MonoBehaviour, IImpactable
{
    public ItemData resourcePref;
    public float health;

    public void ReceiveImpact(float value)
    {
        health -= value;

        if (health <= 0)
        {
            resourcePref.ToDropItem(transform.position + Vector3.up, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
