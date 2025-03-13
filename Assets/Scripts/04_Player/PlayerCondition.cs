using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public StatManager statManager;

    private Condition _health;
    private Condition _hunger;
    private Condition _stamina;

    public float noHungerHealthDecay;

    private void Awake()
    {
        _health = new Condition();
        _health.curValue = statManager.health;
        _health.startValue = statManager.health;   
        _health.maxValue = statManager.health;
        
        _hunger = new Condition();
        _hunger.curValue = statManager.hunger;
        _hunger.startValue = statManager.hunger;
        _hunger.maxValue = statManager.hunger;

        _stamina = new Condition();
        _stamina.curValue = statManager.stamina;
        _stamina.startValue = statManager.stamina;
        _stamina.maxValue = statManager.stamina;
    }

    private void Update()
    {
        _hunger.Subtract(_hunger.passiveValue * Time.deltaTime);
        _stamina.Add(_stamina.passiveValue * Time.deltaTime);

        if (_hunger.curValue < 0f)
        {
            _health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }
    }

    public void Heal(float amount)
    {
        _health.Add(amount);
    }

    public void Eat(float amount)
    {
        _hunger.Add(amount);
    }

    public bool UseStamina(float amount)
    {
        if (_stamina.curValue - amount < 0f)
        {
            return false;
        }
        _stamina.Subtract(amount);
        return true;
    }
}