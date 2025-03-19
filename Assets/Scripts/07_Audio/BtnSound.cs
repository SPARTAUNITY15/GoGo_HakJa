using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnSound : MonoBehaviour
{
    public AudioClip click;

    public void PlaySound()
    {
        AudioManager.Instance.PlayPlayerSound(click);
    }
}
