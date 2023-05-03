using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonobehaviour<SoundManager>
{
    public enum Sound
    {
        magicSplash,
        zombieDeath,
        batDeath,
        maleDeathSound,
        footstep,
    }

    private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> soundAudioClipDictionary;

    protected override void Awake()
    {
        Singleton();
        audioSource = GetComponent<AudioSource>();

        soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();
        foreach (Sound sound in System.Enum.GetValues(typeof(Sound)))
        {
            soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }

    public void PlaySound(Sound sound)
    {
        audioSource.PlayOneShot(soundAudioClipDictionary[sound]);
    }
}
