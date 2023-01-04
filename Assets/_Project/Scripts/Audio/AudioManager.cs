using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager :Singleton<AudioManager>
{
    [SerializeField] private AudioSource m_EffectsSource;
    [SerializeField] private AudioSource m_MusicSource;
    [SerializeField] private AudioClip m_ButtonClick;
    [SerializeField] private AudioClip m_CoinSound;
    [SerializeField] private AudioClip m_UnlockSound;
    private const string c_IsMusicOff = "MusicOff";
    public static Action OnButtonClicked;
    
    private void OnValidate()
    {
        SetRefs();
    }

    public override void Start()
    {
        PlayMusicSound();
        ToggleMusic(PlayerPrefs.GetInt(c_IsMusicOff));
    }
    private void OnEnable()
    {
        OnButtonClicked += PlayButtonSound;
    }
    private void OnDisable()
    {
        OnButtonClicked -= PlayButtonSound;
    }
    [Button]
    private void SetRefs()
    {
        m_MusicSource = transform.FindDeepChild<AudioSource>("MusicSource");
        m_EffectsSource = transform.FindDeepChild<AudioSource>("EffectsSource");
    }
    public void PlayButtonSound()
    {
        m_EffectsSource.PlayOneShot(m_ButtonClick);
    }
    public void PlayUnlockSound()
    {
        m_EffectsSource.PlayOneShot(m_UnlockSound);
    }
    public void PlayCoinSound()
    {
        m_EffectsSource.PlayOneShot(m_CoinSound);
    }

    public void PlayMusicSound()
    {
        m_MusicSource.Play();
    }

    public void ToggleMusic(int intState)
    {
        var state= intState == 0;
        m_MusicSource.enabled = state;
    }
    

    
}
