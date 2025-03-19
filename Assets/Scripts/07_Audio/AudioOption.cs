using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioOption : MonoBehaviour
{
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public AudioClip sfx;

    public Button masterMuteButton;
    public Button bgmMuteButton;
    public Button sfxMuteButton;

    public Sprite normalSprite;
    public Sprite pressedSprite;

    private const string MasterVolumeKey = "MasterVolume";
    private const string BGMVolumeKey = "BGMVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private float prevMasterVolume;
    private float prevBGMVolume;
    private float prevSFXVolume;

    private bool isMasterMuted = false;
    private bool isBGMMuted = false;
    private bool isSFXMuted = false;

    void Start()
    {
        // 저장된 값 불러오기 (없으면 기본값 0.5)
        prevMasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 0.5f);
        prevBGMVolume = PlayerPrefs.GetFloat(BGMVolumeKey, 0.5f);
        prevSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 0.5f);

        // 슬라이더 초기화
        masterSlider.value = prevMasterVolume;
        bgmSlider.value = prevBGMVolume;
        sfxSlider.value = prevSFXVolume;

        // 슬라이더 값 변경 시, 오디오에 즉시 적용 (저장은 안 함)
        masterSlider.onValueChanged.AddListener(PreviewMasterVolume);
        bgmSlider.onValueChanged.AddListener(PreviewBGMVolume);
        sfxSlider.onValueChanged.AddListener(PreviewSFXVolume);

        // 음소거 버튼 이벤트 리스너 추가
        masterMuteButton.onClick.AddListener(ToggleMasterMute);
        bgmMuteButton.onClick.AddListener(ToggleBGMMute);
        sfxMuteButton.onClick.AddListener(ToggleSFXMute);
    }

    // 슬라이더 변경 시, 현재 오디오 볼륨에만 반영 (저장은 안 함)
    public void PreviewMasterVolume(float value)
    {
        if (!isMasterMuted) AudioManager.Instance.SetMasterVolume(value);
    }

    public void PreviewBGMVolume(float value)
    {
        if (!isBGMMuted) AudioManager.Instance.SetBGMVolume(value);
    }

    public void PreviewSFXVolume(float value)
    {
        if (!isSFXMuted) AudioManager.Instance.SetSFXVolume(value);
    }

    //  저장 버튼을 누르면 값이 저장됨
    public void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat(MasterVolumeKey, masterSlider.value);
        PlayerPrefs.SetFloat(BGMVolumeKey, bgmSlider.value);
        PlayerPrefs.SetFloat(SFXVolumeKey, sfxSlider.value);
        PlayerPrefs.Save();

        // 현재 설정을 새로운 이전 값으로 저장
        prevMasterVolume = masterSlider.value;
        prevBGMVolume = bgmSlider.value;
        prevSFXVolume = sfxSlider.value;
    }

    //취소 버튼을 누르면 이전 값으로 되돌림
    public void CancelAudioSettings()
    {
        masterSlider.value = prevMasterVolume;
        bgmSlider.value = prevBGMVolume;
        sfxSlider.value = prevSFXVolume;

        AudioManager.Instance.SetMasterVolume(prevMasterVolume);
        AudioManager.Instance.SetBGMVolume(prevBGMVolume);
        AudioManager.Instance.SetSFXVolume(prevSFXVolume);
    }

    // 기본값 버튼을 누르면 모든 볼륨을 0.5로 설정
    public void ResetAudioSettings()
    {
        float defaultVolume = 0.5f;

        // 볼륨 기본값으로 설정
        masterSlider.value = defaultVolume;
        bgmSlider.value = defaultVolume;
        sfxSlider.value = defaultVolume;

        AudioManager.Instance.SetMasterVolume(defaultVolume);
        AudioManager.Instance.SetBGMVolume(defaultVolume);
        AudioManager.Instance.SetSFXVolume(defaultVolume);

        // 모든 음소거 해제
        isMasterMuted = false;
        isBGMMuted = false;
        isSFXMuted = false;

        // 버튼 이미지 초기화
        UpdateMuteState(masterMuteButton, false);
        UpdateMuteState(bgmMuteButton, false);
        UpdateMuteState(sfxMuteButton, false);
    }

    //  마스터 볼륨 음소거 토글
    public void ToggleMasterMute()
    {
        isMasterMuted = !isMasterMuted;
        UpdateMuteState(masterMuteButton, isMasterMuted);

        if (isMasterMuted)
        {
            AudioManager.Instance.SetMasterVolume(0);
        }
        else
        {
            AudioManager.Instance.SetMasterVolume(masterSlider.value);
        }
    }

    // BGM 볼륨 음소거 토글
    public void ToggleBGMMute()
    {
        isBGMMuted = !isBGMMuted;
        UpdateMuteState(bgmMuteButton, isBGMMuted);

        if (isBGMMuted)
        {
            AudioManager.Instance.SetBGMVolume(0);
        }
        else
        {
            AudioManager.Instance.SetBGMVolume(bgmSlider.value);
        }
    }

    //  SFX 볼륨 음소거 토글
    public void ToggleSFXMute()
    {
        isSFXMuted = !isSFXMuted;
        UpdateMuteState(sfxMuteButton, isSFXMuted);

        if (isSFXMuted)
        {
            AudioManager.Instance.SetSFXVolume(0);
        }
        else
        {
            AudioManager.Instance.SetSFXVolume(sfxSlider.value);
        }
    }

    //음소거 상태일 때 버튼 스프라이트 변경
    private void UpdateMuteState(Button button, bool isMuted)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? pressedSprite : normalSprite;
        }
    }
}
