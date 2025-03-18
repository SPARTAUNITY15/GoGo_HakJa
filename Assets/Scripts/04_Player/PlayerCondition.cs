using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCondition : StatManager
{
    public float curHealth;
    [HideInInspector] public float maxHealth;
    [HideInInspector] public float startHealth;

    public float curHunger;
    [HideInInspector] public float maxHunger;
    [HideInInspector] public float startHunger;
    [HideInInspector] public float passiveHunger;

    public float curStamina;
    [HideInInspector] public float maxStamina;
    [HideInInspector] public float startStamina;
    [HideInInspector] public float passiveStamina;

    public float curMoisture;
    [HideInInspector] public float maxMoisture;
    [HideInInspector] public float startMoisture;
    [HideInInspector] public float passiveMoisture;

    [HideInInspector] public bool useStamina = false;

    [SerializeField]
    public float noHungerHealthDecay;
    public float noHungerMoistureDecay;

    public event Action onTakeDamage;
    private Animator animator;

    public void Awake()
    {
        curHealth = health;
        maxHealth = health;
        startHealth = health;

        curHunger = hunger;
        maxHunger = hunger;
        startHunger = hunger;
        passiveHunger = 3f;

        curStamina = stamina;
        maxStamina = stamina;
        startStamina = stamina;
        passiveStamina = 5f;

        curMoisture = moisture;
        maxMoisture = moisture;
        startMoisture = moisture;
        passiveMoisture = 2f;

        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        curHunger = Mathf.Max(curHunger - passiveHunger * Time.deltaTime, 0f);
        curMoisture = Mathf.Max(curMoisture - passiveMoisture * Time.deltaTime, 0f);
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
        if (curMoisture <= 0f)
        {
            curMoisture = Mathf.Max(curMoisture - noHungerHealthDecay * Time.deltaTime, 0f);
        }


        LowHealth();
    }

    public void Heal(float amount)
    {
        curHealth = Mathf.Min(curHealth + amount, maxHealth);
    }

    public void Eat(float amount)
    {
        curHunger = Mathf.Min(curHunger + amount, maxHunger);
    }

    public void Drink(float amount)
    {
        curMoisture = Mathf.Min(curMoisture + amount, maxMoisture);
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

    // DamageIndicator 사용전용, 데미지 감소 실 테스트 필요, 추후 다른곳에서 호출 필요
    public void TakePhysicalDamage(float damage)
    {
        onTakeDamage?.Invoke();
        animator.SetTrigger("IsHit");
        curHealth -= damage;
        if (curHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        animator.SetTrigger("IsDie");
        Destroy(gameObject, 5f);
    }

    // 체력감소시 DamageIndicator 사용
    public void LowHealth()
    {
        if (curHealth <= maxHealth * 0.15f)
        {
            onTakeDamage?.Invoke();
        }
    }

    public void UseConsumableItem(ItemData item)
    {
        foreach (ItemData_Consumable effect in item.ItemData_Consumables)
        {
            switch(effect.consumableType)
            {
                case ConsumableType.Stamina:
                    Debug.Log("스태미나 회복은 기획에 x");
                    break;
                case ConsumableType.Thirst:
                    Drink(effect.value);
                    break;
                case ConsumableType.Temperature:
                    Debug.Log("체온 변동은 기획에 x");
                    break;
                case ConsumableType.Hunger:
                    Eat(effect.value);
                    break;
                case ConsumableType.Health:
                    Heal(effect.value);
                    break;
            }
        }
    }
}

