using System;
using UnityEngine;
using UnityEngine.Audio;

public class AssetsManager : Singleton<AssetsManager>
{
    public AudioMixer MainAudioMixer;
    public AudioMixerSnapshot PlayerIsNotTakingDamageSnapshot;
    public AudioMixerSnapshot PlayerIsTakingDamageSnapshot;
    public AudioMixerGroup MasterAudioMixerGroup;
    public AudioMixerGroup SoundsAudioMixerGroup;
    public AudioMixerGroup MusicAudioMixerGroup;
    public SoundAudioClip[] SoundAudioClips;

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound Sound;
        public AudioClip AudioClip;
    }
}