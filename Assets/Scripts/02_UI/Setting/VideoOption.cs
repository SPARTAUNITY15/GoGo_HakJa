using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VideoOption : MonoBehaviour
{
    FullScreenMode screenMode;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenBtn;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum;

    void Start()
    {
        InitUI();
        LoadSettings(); 
    }

    void InitUI()
    {
        resolutions.Clear();
        resolutionDropdown.options.Clear();

        // 현재 해상도 가져오기
        Resolution currentResolution = Screen.currentResolution;

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            resolutions.Add(Screen.resolutions[i]);
        }

        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData
            {
                text = item.width + "x" + item.height + " " + item.refreshRate + "Hz"
            };
            resolutionDropdown.options.Add(option);

            if (item.width == currentResolution.width && item.height == currentResolution.height)
                resolutionNum = optionNum;

            optionNum++;
        }

        resolutionDropdown.value = resolutionNum;
        resolutionDropdown.RefreshShownValue();

        fullscreenBtn.isOn = Screen.fullScreenMode == FullScreenMode.FullScreenWindow;
        screenMode = Screen.fullScreenMode;
    }
    public void GoLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }
    public void DropboxOptionChange(int x)
    {
        if (x >= 0 && x < resolutions.Count)
        {
            resolutionNum = x;
        }
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OkBtnClick()
    {
        if (resolutionNum >= 0 && resolutionNum < resolutions.Count)
        {
            Screen.SetResolution(
                resolutions[resolutionNum].width,
                resolutions[resolutionNum].height,
                screenMode
            );

            SaveSettings();
        }
        else
        {
            Debug.LogWarning("해상도 설정 오류: 유효한 해상도가 선택되지 않았습니다.");
        }
    }

    void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", resolutionNum);
        PlayerPrefs.SetInt("Fullscreen", fullscreenBtn.isOn ? 1 : 0);

        // 현재 해상도를 저장하여 CanvasAutoScaler가 읽을 수 있도록 추가
        PlayerPrefs.SetInt("SavedResolutionWidth", resolutions[resolutionNum].width);
        PlayerPrefs.SetInt("SavedResolutionHeight", resolutions[resolutionNum].height);

        PlayerPrefs.Save();
    }


    void LoadSettings()
    {
        resolutionNum = PlayerPrefs.GetInt("ResolutionIndex", resolutionNum);
        fullscreenBtn.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        resolutionDropdown.value = resolutionNum;
        resolutionDropdown.RefreshShownValue();

        Screen.SetResolution(
            resolutions[resolutionNum].width,
            resolutions[resolutionNum].height,
            fullscreenBtn.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed
        );
    }
}
