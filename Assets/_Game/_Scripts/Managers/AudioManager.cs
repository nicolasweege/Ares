using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private SoundController[] _sounds;

    protected override void Awake()
    {
        foreach (SoundController sc in _sounds)
        {
            sc.Source = gameObject.AddComponent<AudioSource>();
            sc.Source.clip = sc.Clip;
            sc.Source.volume = sc.Volume;
            sc.Source.pitch = sc.Pitch;
        }
    }

    public void Play(string soundName)
    {
        SoundController sc = Array.Find(_sounds, sound => sound.Name == soundName);
        sc.Source?.Play();
        sc.Volume = 0.3f;
    }
}