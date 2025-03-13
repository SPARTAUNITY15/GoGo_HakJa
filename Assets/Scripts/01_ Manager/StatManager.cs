using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}

public class StatManager : MonoBehaviour, IDamagable
{
    [SerializeField] public float health = 3f;
    [SerializeField] public float hunger = 3f;
    [SerializeField] public float stamina = 3f;
    [SerializeField] public float moisture = 3f;
    [SerializeField] public float temperature = 3f;
    [SerializeField] public float speed;

    public PlayerCondition playerCondition;


    // ΩÃ±€≈Ê
    // 

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
