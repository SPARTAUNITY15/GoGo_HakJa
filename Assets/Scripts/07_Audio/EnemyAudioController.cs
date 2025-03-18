using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    private List<AudioSource> audioSources;

    private void Awake()
    {
        audioSources = new List<AudioSource>();
        for (int i = 0; i < 10; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(audioSource);
        }
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        SetVolume(sfxVolume);
    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource audioSource = GetAvailableAudioSource();
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }
        return null;
    }

    public void SetVolume(float volume)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = volume;
        }
    }
}
