using System;
using UnityEngine;

public class AssetsManager : Singleton<AssetsManager>
{
    public SoundAudioClip[] SoundAudioClips;

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound Sound;
        public AudioClip AudioClip;
    }
}