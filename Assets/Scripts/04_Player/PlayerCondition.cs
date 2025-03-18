using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCondition : StatManager
{
    PlayerController playerController;

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
    public GameObject deathPanel;  //����� Ȱ��ȭ�� �г�
    UIManager uimanager;
    Rigidbody rb;

    float Hitdelay = 5f;

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
        passiveStamina = 3f;

        curMoisture = moisture;
        maxMoisture = moisture;
        startMoisture = moisture;
        passiveMoisture = 2f;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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

        Hitdelay -= Time.deltaTime;
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

    public void Stop()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    // DamageIndicator �������, ������ ���� �� �׽�Ʈ �ʿ�, ���� �ٸ������� ȣ�� �ʿ�
    public void TakePhysicalDamage(float damage)
    {
        onTakeDamage?.Invoke();
        animator.SetTrigger("IsHit");
        Stop();
        curHealth -= damage;
        if (curHealth <= 0)
        {
            Die();
        }
        if(Hitdelay <= 0)
        {
            playerController.SlowSpeed();
        }
    }

    public void Die()
    {
        animator.SetBool("IsDie", true);
        uimanager.ToggleCursor();
        deathPanel.SetActive(true);
        TimerManager.instance.StopTimer();
        Destroy(gameObject, 5f);
    }

    // ü�°��ҽ� DamageIndicator ���
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
            switch (effect.consumableType)
            {
                case ConsumableType.Stamina:
                    Debug.Log("���¹̳� ȸ���� ��ȹ�� x");
                    break;
                case ConsumableType.Thirst:
                    Drink(effect.value);
                    break;
                case ConsumableType.Temperature:
                    Debug.Log("ü�� ������ ��ȹ�� x");
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

