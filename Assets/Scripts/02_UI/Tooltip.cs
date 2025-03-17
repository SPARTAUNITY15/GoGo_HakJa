using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public static Tooltip Instance;
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipName;
    public TextMeshProUGUI tooltipDesc;
    public Image Image;
    private RectTransform rectTransform;
    private Vector3 offset = new Vector3(10f, -10f, 0f); // 마우스와의 거리 조정

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        rectTransform = tooltipPanel.GetComponent<RectTransform>();
        tooltipPanel.SetActive(false);
    }

    private void Update()
    {
        if (tooltipPanel.activeSelf)
        {
            // 툴팁 위치를 마우스 커서 근처로 이동
            Vector3 newPos = Input.mousePosition + offset;
            newPos.z = 0f;
            rectTransform.position = newPos;
        }
    }

    public void ShowTooltip(string name, string description)
    {
        tooltipPanel.SetActive(true);
        tooltipName.text = name;
        tooltipDesc.text = description;
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
