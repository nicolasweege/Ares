using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer _mainMixer;
    private Resolution[] _resolutions;
    public Dropdown ResolutionsDropdown;

    private void Start() {
        _resolutions = Screen.resolutions;
        ResolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);
        }

        ResolutionsDropdown.AddOptions(options);
    }

    public void SetMasterVolume(float masterVolume)
    {
        _mainMixer.SetFloat("MasterVolume", masterVolume);
        _mainMixer.SetFloat("SoundsVolume", masterVolume);
        _mainMixer.SetFloat("MusicVolume", masterVolume);
    }

    public void SetSoundsVolume(float soundsVolume)
    {
        _mainMixer.SetFloat("SoundsVolume", soundsVolume);
    }

    public void SetMusicVolume(float musicVolume)
    {
        _mainMixer.SetFloat("MusicVolume", musicVolume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}