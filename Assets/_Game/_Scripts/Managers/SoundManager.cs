using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        PlayerShoot,
        PlayerTakingDamage,
        TestSound
    }

    private static GameObject _oneShotGameObject;
    private static AudioSource _oneShotAudioSource;

    private static Dictionary<Sound, float> _soundTimerDictionary;

    private static void InitializeDictionary()
    {
        // SoundManager.InitializeDictionary(); call it when start the game or a scene, idk yet
        _soundTimerDictionary = new Dictionary<Sound, float>();
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            case Sound.TestSound:
                if (_soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = _soundTimerDictionary[sound];
                    float maxTimer = 0.15f;
                    if (lastTimePlayed + maxTimer < Time.time)
                    {
                        _soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else return false;
                }
                else return true;

            default:
                return true;
        }
    }

    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            if (_oneShotGameObject == null)
            {
                _oneShotGameObject = new GameObject("One Shot Sound");
                _oneShotAudioSource = _oneShotGameObject.AddComponent<AudioSource>();
            }
            _oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    public static void PlaySound(Sound sound, Vector3 pos)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("One Shot Sound");
            soundGameObject.transform.position = pos;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.maxDistance = 100f;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;
            audioSource.volume = 0.1f;
            audioSource.Play();

            UnityEngine.Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    public static AudioClip GetAudioClip(Sound sound)
    {
        foreach (AssetsManager.SoundAudioClip soundAudioClip in AssetsManager.Instance.SoundAudioClips)
        {
            if (soundAudioClip.Sound == sound)
                return soundAudioClip.AudioClip;
        }
        Debug.LogError($"Sound {sound} not found");
        return null;
    }
}