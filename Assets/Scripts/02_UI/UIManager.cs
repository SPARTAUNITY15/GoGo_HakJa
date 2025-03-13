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
    }

    public List<UIElement> uiElements; 
    private Dictionary<string, GameObject> uiDictionary = new Dictionary<string, GameObject>();

    private GameObject currentActiveUI = null; // 현재 활성화된 UI

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
            GameObject uiInstance = Instantiate(element.uiPrefab, transform);
            uiInstance.SetActive(false); 
            uiDictionary[element.uiName] = uiInstance;
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
