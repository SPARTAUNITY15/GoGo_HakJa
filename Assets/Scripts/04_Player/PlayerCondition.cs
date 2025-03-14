using System;
using UnityEngine;

public class PlayerCondition : StatManager
{
    public float curHealth;
    [HideInInspector] public float maxHealth;
    [HideInInspector] public float startHealth;

    public float curHunger;
    [HideInInspector] public float maxHunger;
    [HideInInspector] public float startHunger;

    public float curStamina;
    [HideInInspector] public float maxStamina;
    [HideInInspector] public float startStamina;
    public float passiveStamina;

    [HideInInspector] public bool useStamina = false;

    [SerializeField]
    public float noHungerHealthDecay;

    public void Awake()
    {
        curHealth = health;
        maxHealth = health;
        startHealth = health;

        curHunger = hunger;
        maxHunger = hunger;
        startHunger = hunger;

        curStamina = stamina;
        maxStamina = stamina;
        startStamina = stamina;
        passiveStamina = 5f;
    }

    public void Update()
    {
        curHunger = Mathf.Max(curHunger - passiveStamina * Time.deltaTime, 0f);
        if (useStamina)
        {
            curStamina = Mathf.Max(curStamina - passiveStamina * Time.deltaTime, 0f);
        }
        else
        {
            curStamina = Mathf.Min(curStamina + passiveStamina * Time.deltaTime, maxStamina);
        }

        if (curHunger <= 0f)
        {
            curHealth = Mathf.Max(curHealth - noHungerHealthDecay * Time.deltaTime, 0f);
        }
    }

    public void Heal(float amount)
    {
        curHealth = Mathf.Min(curHealth + amount, maxHealth);
    }

    public void Eat(float amount)
    {
        curHunger = Mathf.Min(curHunger + amount, maxHunger);
    }

    public bool UseStamina(float amount)
    {
        if (curStamina - amount < 0f)
        {
            return false;
        }
        curStamina -= amount;
        return true;
    }

    public void LoseStamina()
    {
        useStamina = !useStamina;
    }
}

