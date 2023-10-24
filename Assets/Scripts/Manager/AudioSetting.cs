using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : SingletonMonobehaviour<AudioSetting>
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    public enum SFXSound
    {
        magicSplash,
        zombieDeath,
        batDeath,
        maleDeathSound,
        footstep,
    }
    public enum musicSound
    {
        BGMusic,
    }
    private Dictionary<SFXSound, AudioClip> SFXSoundAudioClipDictionary;
    private Dictionary<musicSound, AudioClip> musicSoundAudioClipDictionary;

    protected override void Awake()
    {
        base.Awake();

        SFXSoundAudioClipDictionary = new Dictionary<SFXSound, AudioClip>();
        foreach (SFXSound sound in System.Enum.GetValues(typeof(SFXSound)))
        {
            SFXSoundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }

        musicSoundAudioClipDictionary = new Dictionary<musicSound, AudioClip>();
        foreach (musicSound sound in System.Enum.GetValues(typeof(musicSound)))
        {
            musicSoundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume") || PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume();
        }
        else
        {
            setMasterVolume();
            setMusicVolume();
            setSFXVolume();
        }
    }
    public void setMasterVolume()
    {
        float volume = masterSlider.value;
        myMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    public void setMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void setSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
            setMasterVolume();
        }
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            setMusicVolume();
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            setSFXVolume();
        }
    }
    public void playMusicClip(musicSound sound)
    {
        musicSource.clip = musicSoundAudioClipDictionary[sound];
        musicSource.Play();
    }
    public void playSFXClip(SFXSound sound)
    {
        SFXSource.clip = SFXSoundAudioClipDictionary[sound];
        SFXSource.Play();
    }
}
