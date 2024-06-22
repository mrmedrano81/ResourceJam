using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://www.youtube.com/watch?v=QuXqyHpquLg&t=7s")]
public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [Header("References")]
    [SerializeField] private AudioSource audioSource, musicSource;

    [Header("Music")]
    [SerializeField] private AudioClip[] musicList;

    [Header("SFX")]
    [SerializeField] private AudioClip[] sfxList;

    [Header("ButtonSFX")]
    [SerializeField] private AudioClip[] buttonSfxList;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(int musicNumber)
    {
        musicSource.clip = musicList[musicNumber];
        musicSource.Play();
    }

    public void PlaySFX(int sfxNumber)
    {
        audioSource.clip = sfxList[sfxNumber];
        audioSource.Play();
    }

    public void PlayButtonSFX(int buttonNumber)
    {
        audioSource.clip = buttonSfxList[buttonNumber];
        audioSource.Play();
    }
}
