using System;
using UnityEngine;

public class PlayerCondition : StatManager
{
    private Condition _health;
    private Condition _hunger;
    private Condition _stamina;

    public float noHungerHealthDecay;

    private void Awake()
    {
        _health = new Condition();
        _health.curValue = health;
        _health.startValue = health;   
        _health.maxValue = health;
        
        _hunger = new Condition();
        _hunger.curValue = hunger;
        _hunger.startValue = hunger;
        _hunger.maxValue = hunger;

        _stamina = new Condition();
        _stamina.curValue = stamina;
        _stamina.startValue = stamina;
        _stamina.maxValue = stamina;
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