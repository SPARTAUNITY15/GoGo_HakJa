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
        // 현재값은 시작값, 나중엔 다른 저장값으로 넣어줘도 됨
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
        // FillAmount에 넣어줄 값을 구해주기 위해, 0~1사이의 수를 계산
        return curValue / maxValue;
    }
}
