using UnityEngine;
using UnityEngine.UI;

public class StartToggle : MonoBehaviour
{
    public Toggle myToggle; // Unity에서 연결할 토글 UI
    public Image toggleImage; // 토글 버튼의 이미지
    public Sprite pressedSprite; // 눌린 상태의 이미지

    void Start()
    {
        myToggle.isOn = true; // 시작부터 눌린 상태
        toggleImage.sprite = pressedSprite; // 눌린 이미지로 변경
    }
}