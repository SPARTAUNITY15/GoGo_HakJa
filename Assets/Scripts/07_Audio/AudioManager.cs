using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    // 사운드 컨트롤러
    private PlayerAudioController playerAudioController;
    private BGMController bgmController;
    private EnemyAudioController enemyAudioController;

    // 볼륨 키 값
    private const string MasterVolumeKey = "MasterVolume";
    private const string BGMVolumeKey = "BGMVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private float masterVolume = 1.0f; // 기본값

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 오디오 컨트롤러 초기화
            playerAudioController = gameObject.AddComponent<PlayerAudioController>();
            bgmController = gameObject.AddComponent<BGMController>();
            enemyAudioController = gameObject.AddComponent<EnemyAudioController>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 저장된 볼륨값 불러오기 (없으면 기본값 0.5)
        masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 0.5f);
        float bgmVolume = PlayerPrefs.GetFloat(BGMVolumeKey, 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 0.5f);

        SetMasterVolume(masterVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);
    }

    // 마스터 볼륨 설정 (BGM 및 SFX에 영향)
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        SetBGMVolume(PlayerPrefs.GetFloat(BGMVolumeKey, 0.5f)); // BGM 반영
        SetSFXVolume(PlayerPrefs.GetFloat(SFXVolumeKey, 0.5f)); // SFX 반영

        PlayerPrefs.SetFloat(MasterVolumeKey, volume);
        PlayerPrefs.Save();
    }

    // BGM 볼륨 설정 (마스터 볼륨과 곱해짐)
    public void SetBGMVolume(float volume)
    {
        float finalVolume = volume * masterVolume;
        bgmController.SetVolume(finalVolume);
        PlayerPrefs.SetFloat(BGMVolumeKey, volume);
        PlayerPrefs.Save();
    }

    // SFX 볼륨 설정 (마스터 볼륨과 곱해짐)
    public void SetSFXVolume(float volume)
    {
        float finalVolume = volume * masterVolume;
        playerAudioController.SetVolume(finalVolume);
        enemyAudioController.SetVolume(finalVolume);
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
        PlayerPrefs.Save();
    }

    // 플레이어 효과음 재생
    public void PlayPlayerSound(AudioClip clip)
    {
        playerAudioController.PlaySound(clip);
    }

    // BGM 재생
    public void PlayBGM(AudioClip clip)
    {
        bgmController.PlayMusic(clip);
    }

    // 적 효과음 재생
    public void PlayEnemySound(AudioClip clip)
    {
        enemyAudioController.PlaySound(clip);
    }
}
