using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(float damage);
}

public class StatManager : MonoBehaviour
{
    public PlayerCondition playerCondition;

    [SerializeField] public float health;
    [SerializeField] public float hunger;
    [SerializeField] public float stamina;
    [SerializeField] public float moisture;
    [SerializeField] public float temperature;
    [SerializeField] public float speed;

    public void AddStat(ref float currentValue, float amount, float maxValue)
    {
        currentValue = Mathf.Min(currentValue + amount, maxValue);
    }
}
