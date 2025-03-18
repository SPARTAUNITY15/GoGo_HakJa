using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject volumePanel;   // 볼륨 패널
    public GameObject resolutionPanel; // 해상도 패널
    void Start()
    {
     // 처음에는 ㅂㅗ륨 패널만 활성화
        ShowVolumePanel();
    }

    public void ShowVolumePanel()
    {
        volumePanel.SetActive(true);
        resolutionPanel.SetActive(false);
    }

    public void ShowResolutionPanel()
    {
        volumePanel.SetActive(false);
        resolutionPanel.SetActive(true);
    }
}
