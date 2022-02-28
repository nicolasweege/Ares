using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer MainMixer;
    public void SetMasterVolume(float masterVolume)
    {
        MainMixer.SetFloat("MasterVolume", masterVolume);
    }

    public void SetSoundsVolume(float soundsVolume)
    {
        MainMixer.SetFloat("SoundsVolume", soundsVolume);
    }

    public void SetMusicVolume(float musicVolume)
    {
        MainMixer.SetFloat("MusicVolume", musicVolume);
    }
}