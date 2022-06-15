using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundController
{
    [SerializeField] private string _name;
    [SerializeField] private AudioClip _clip;

    [Range(0f, 1f)]
    [SerializeField] private float _volume;

    [Range(0.1f, 3f)]
    [SerializeField] private float _pitch;
    private AudioSource _source;

    public string Name { get => _name; }
    public AudioClip Clip { get => _clip; set => _clip = value; }
    public float Volume { get => _volume; set => _volume = value; }
    public float Pitch { get => _pitch; set => _pitch = value; }
    public AudioSource Source { get => _source; set => _source = value; }
}