using UnityEngine;
using UnityEngine.UI;

public class ConditionUI : MonoBehaviour
{
    public PlayerCondition playerCondition;

    public Image _Health;
    public Image _Hunger;
    public Image _Stamina;
    public Image _Moisture;

    public string conditionType; // "Health", "Hunger", "Stamina", "Moisture" 중 하나

    private void Update()
    {
        if (playerCondition == null)
        {
            playerCondition = GetComponentInParent<PlayerCondition>();

            if (playerCondition == null)
            {
                Debug.LogError("PlayerCondition을 찾을 수 없습니다!");
                return;
            }
        }

        if (_Health != null)
            _Health.fillAmount = SafeDivide(playerCondition.curHealth, playerCondition.maxHealth);
        if (_Hunger != null)
            _Hunger.fillAmount = SafeDivide(playerCondition.curHunger, playerCondition.maxHunger);
        if (_Stamina != null)
            _Stamina.fillAmount = SafeDivide(playerCondition.curStamina, playerCondition.maxStamina);
        if (_Moisture != null)
            _Moisture.fillAmount = SafeDivide(playerCondition.curMoisture, playerCondition.maxMoisture);
    }

    //if (_Health != null) 
    //    _Health.fillAmount = playerCondition.curHealth / playerCondition.maxHealth;
    //if (_Hunger != null)
    //    _Hunger.fillAmount = playerCondition.curHunger / playerCondition.maxHunger;
    //if (_Stamina != null)
    //    _Stamina.fillAmount = playerCondition.curStamina / playerCondition.maxStamina;
    //if (_Moisture != null)
    //    _Moisture.fillAmount = playerCondition.curMoisture / playerCondition.maxMoisture;
    private float SafeDivide(float numerator, float denominator)
    {
        if (denominator == 0)
        {
            Debug.LogError("0으로 나누기를 시도했습니다!");
            return 0f;
        }
        return Mathf.Clamp01(numerator / denominator); // 0~1 범위로 클램프
    }
}
