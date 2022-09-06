using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class SoundManager
{
    public enum Sound
    {
        #region Player
            PlayerShoot,
            PlayerTakingDamage,
        #endregion

        #region Afrodite
            AfroditeSecondStageLaserShoot,
            AfroditeFirstStageShoot,
            AfroditeThirdStageShoot,
            AfroditeLaserShootAntecipation,
            AfroditeLaserLockOn,
        #endregion

        #region UI
            UIButtonSelection,
            UIButtonClick,
            UIButtonLightClick,
        #endregion

        #region Eros
            ErosShoot_1,
        #endregion

        TestSound
    }

    private static GameObject _oneShotGameObject;
    private static AudioSource _oneShotAudioSource;

    private static Dictionary<Sound, float> _soundTimerDictionary;

    private static void InitializeDictionary()
    {
        // call it when start the game or a scene
        // SoundManager.InitializeDictionary();
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
                _oneShotAudioSource.outputAudioMixerGroup = AssetsManager.Instance.SoundsAudioMixerGroup;
            }
            _oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    public static void PlaySound(Sound sound, float volume = 1f)
    {
        if (CanPlaySound(sound))
        {
            if (_oneShotGameObject == null)
            {
                _oneShotGameObject = new GameObject("One Shot Sound");
                _oneShotAudioSource = _oneShotGameObject.AddComponent<AudioSource>();
                _oneShotAudioSource.volume = volume;
                _oneShotAudioSource.outputAudioMixerGroup = AssetsManager.Instance.SoundsAudioMixerGroup;
            }
            _oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    public static void PlaySound(Sound sound, Vector3 pos, float volume = 1f)
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
            audioSource.volume = volume;
            audioSource.priority = 100;
            audioSource.outputAudioMixerGroup = AssetsManager.Instance.SoundsAudioMixerGroup;
            audioSource.Play();

            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    public static void PlaySound(Sound sound, Vector3 pos, float volume = 1f, int priority = 0)
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
            audioSource.volume = volume;
            audioSource.priority = priority;
            audioSource.outputAudioMixerGroup = AssetsManager.Instance.SoundsAudioMixerGroup;
            audioSource.Play();

            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    public static void PlaySound(Sound sound, Vector3 pos, AudioMixerGroup audioMixerGroup, float volume = 1f, int priority = 0)
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
            audioSource.volume = volume;
            audioSource.priority = priority;
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.Play();

            Object.Destroy(soundGameObject, audioSource.clip.length);
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