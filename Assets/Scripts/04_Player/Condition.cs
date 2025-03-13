using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public float passiveValue;

    private void Start()
    {
        // ���簪�� ���۰�, ���߿� �ٸ� ���尪���� �־��൵ ��
        curValue = startValue;
    }

    public void Set(float value)
    {
        curValue = value;
    }

    public void Add(float Value)
    {
        curValue = Mathf.Min(curValue + Value, maxValue);
    }

    public void Subtract(float Value)
    {
        curValue = Mathf.Max(curValue - Value, 0.0f);
    }

    public float GetPercentage()
    {
        // FillAmount�� �־��� ���� �����ֱ� ����, 0~1������ ���� ���
        return curValue / maxValue;
    }
}
