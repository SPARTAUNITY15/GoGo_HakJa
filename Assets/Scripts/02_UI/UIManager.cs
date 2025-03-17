using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [System.Serializable]
    public class UIElement
    {
        public string uiName;
        public GameObject uiPrefab; // UI 프리팹
        public GameObject targetCanvas; // 들어갈 캔버스
    }

    public List<UIElement> uiElements;
    private Dictionary<string, GameObject> uiDictionary = new Dictionary<string, GameObject>();

    private GameObject currentActiveUI = null;// 현재 활성화된 UI

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeUI();
    }

    void InitializeUI()
    {
        foreach (var element in uiElements)
        {
            if (element.targetCanvas == null)
            {
                Debug.LogWarning($"UI '{element.uiName}'의 targetCanvas가 지정되지 않았습니다.");
                continue;
            }

            GameObject uiInstance = Instantiate(element.uiPrefab, element.targetCanvas.transform); // 지정된 캔버스에 추가
            uiInstance.SetActive(false);
            uiDictionary[element.uiName] = uiInstance;
        }
    }

    public void ToggleUI(string uiName) //현재 활성 UI가 null이면 그냥 켜고, null이 아니면 현재 활성 UI랑 미래 UI랑 비교해서 같으면 끄고 다르면 켜진거 끄고 켜기.
    {
        if (currentActiveUI == uiDictionary[uiName])
        {
            HideCurrentUI();
        }
        else
        {
            HideCurrentUI();
            ShowUI(uiName);
        }
    }
    public void ShowUI(string uiName)
    {
        if (uiDictionary.ContainsKey(uiName))
        {
            if (currentActiveUI != null)
            {
                currentActiveUI.SetActive(false); // 기존 UI 비활성화
            }

            currentActiveUI = uiDictionary[uiName];
            currentActiveUI.SetActive(true);
        }
    }

    public void HideCurrentUI()
    {
        if (currentActiveUI != null)
        {
            currentActiveUI.SetActive(false);
            currentActiveUI = null;
        }
    }
}
