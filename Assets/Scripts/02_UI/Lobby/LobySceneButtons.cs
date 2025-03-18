using UnityEngine;
using UnityEngine.SceneManagement;

public class LobySceneButtons : MonoBehaviour
{
    public GameObject optionsPanel; // 옵션 패널

    // Start 버튼을 눌렀을 때 실행되는 함수
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene"); // MainScene으로 이동
    }

    // Exit 버튼을 눌렀을 때 실행되는 함수
    public void ExitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit(); // 게임 종료
    }

    // 옵션 패널 열기
    public void OpenOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Options Panel이 설정되지 않았습니다!");
        }
    }

    // 옵션 패널 닫기
    public void CloseOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }
}
