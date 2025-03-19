using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmSound : MonoBehaviour
{
    public AudioClip _LobbyScene;

    public void Start()
    {
        PlayLobby();
    }

    public void PlayLobby()
    {
        AudioManager.Instance.PlayBGM(_LobbyScene);
    }
}
