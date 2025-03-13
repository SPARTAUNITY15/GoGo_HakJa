using UnityEngine;
using UnityEngine.UI;

public class CanvasAutoScaler : MonoBehaviour
{
    private Vector2 defaultResolution = new Vector2(1920, 1080); // 기본 기준 해상도
    private Vector2 savedResolution; // 저장된 해상도 값

    void Start()
    {
        LoadResolution();
        UpdateCanvasScalers();
    }

    void LoadResolution()
    {
        // 저장된 해상도 값 불러오기 (없으면 기본값 사용)
        float width = PlayerPrefs.GetInt("SavedResolutionWidth", (int)defaultResolution.x);
        float height = PlayerPrefs.GetInt("SavedResolutionHeight", (int)defaultResolution.y);
        savedResolution = new Vector2(width, height);
    }

    public void UpdateCanvasScalers()
    {
        CanvasScaler[] canvasScalers = FindObjectsOfType<CanvasScaler>();

        foreach (CanvasScaler scaler in canvasScalers)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = savedResolution; // 저장된 해상도를 적용
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        }
    }
}
