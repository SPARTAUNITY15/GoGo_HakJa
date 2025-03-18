using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;
    private float elapsedTime;
    private bool isRunning = true;
    public TMP_Text timerText; // UI에 표시할 TMP 텍스트

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = $"생존시간: {elapsedTime:F2} 초";
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
