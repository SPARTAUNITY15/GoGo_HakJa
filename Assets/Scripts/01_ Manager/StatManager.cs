using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}

public class StatManager : MonoBehaviour, IDamagable
{
    [SerializeField] public float health;
    [SerializeField] public float hunger;
    [SerializeField] public float stamina;
    [SerializeField] public float moisture;
    [SerializeField] public float temperature;
    [SerializeField] public float speed;

    public PlayerCondition playerCondition;

    public void TakePhysicalDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
}
