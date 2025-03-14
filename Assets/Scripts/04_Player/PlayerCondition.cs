using System;
using UnityEngine;

public class PlayerCondition : StatManager
{
    public float curHealth;
    public float maxHealth;
    public float startHealth;

    public float curHunger;
    public float maxHunger;
    public float startHunger;

    public float curStamina;
    public float maxStamina;
    public float startStamina;
    public float passiveStamina;

    [SerializeField]
    public float noHungerHealthDecay;

    private void Awake()
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
        passiveStamina = 10f;
    }

    private void Update()
    {
        curHunger = Mathf.Max(curHunger - passiveStamina * Time.deltaTime, 0f);
        curStamina = Mathf.Min(curStamina + passiveStamina * Time.deltaTime, maxStamina);

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

    public void AddHealth(float amount) => AddStat(ref curHealth, amount, maxHealth);
    public void AddHunger(float amount) => AddStat(ref curHunger, amount, maxHunger);
    public void AddStamina(float amount) => AddStat(ref curStamina, amount, maxStamina);
}

