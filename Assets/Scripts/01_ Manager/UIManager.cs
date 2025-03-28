using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public InventoryUI inventoryUI;
    public PromptUI promptUI;
    public PlayerController playerController;

    [System.Serializable]
    public class UIElement
    {
        public string uiName;
        public GameObject uiPrefab; // UI 프리팹
        public GameObject targetCanvas; // 들어갈 캔버스
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
            return;
        }

        playerController = FindObjectOfType<PlayerController>(); // PlayerController 인스턴스 초기화

        if (playerController == null)
        {
            Debug.LogError("PlayerController 인스턴스를 찾을 수 없습니다.");
        }
    }

    private void Start()
    {
        InitializeUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCursor();
            ToggleUI("인벤토리");

            if (inventoryUI != null)
            {
                Instance.inventoryUI.SetCraftMode(CraftMode.Inventory);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentActiveUI != null && currentActiveUI == uiDictionary["인벤토리"])
            {
                // 인벤토리가 열려 있으면 UI만 닫고, 커서는 유지
                HideCurrentUI();
            }
            else
            {
                // 일반적인 ESC 동작 수행 (커서 토글 및 옵션 창 토글)
                ToggleCursor();
                ToggleUI("옵션");
            }
        }
    }
    private void LateUpdate()
    {
        if (playerController != null)
        {
            if (playerController.canLook)
            {
                playerController.CameraLook();
            }
        }
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

        if (uiDictionary.ContainsKey("인벤토리"))
        {
            inventoryUI = uiDictionary["인벤토리"].GetComponent<InventoryUI>();
        }
    }

    public void ToggleUI(string uiName)
    {
        if (uiDictionary.ContainsKey(uiName))
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
        else
        {
            Debug.LogWarning($"UI 이름 '{uiName}'이(가) 존재하지 않습니다.");
        }
    }

    public void ShowUI(string uiName)
    {
        if (uiDictionary.ContainsKey(uiName))
        {
            if (currentActiveUI != null)
            {
                currentActiveUI.SetActive(false);
            }

            currentActiveUI = uiDictionary[uiName];
            currentActiveUI.SetActive(true);
        }
    }
    public void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        playerController.canLook = !toggle;
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
