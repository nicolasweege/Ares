using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UISettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer _mainMixer;
    // [SerializeField] private Dropdown _resolutionsDropdown;
    private Resolution[] _resolutions;

    private void Start()
    {
        // HandleResolutions();
    }

    // private void HandleResolutions(){
    //     _resolutions = Screen.resolutions;
    //     _resolutionsDropdown.ClearOptions();

    //     List<string> options = new List<string>();
    //     int currentResIndex = 0;

    //     for (int i = 0; i < _resolutions.Length; i++)
    //     {
    //         string option = _resolutions[i].width + "x" + _resolutions[i].height;
    //         options.Add(option);
    //         if (_resolutions[i].width == Screen.currentResolution.width
    //             && _resolutions[i].height == Screen.currentResolution.height)
    //             currentResIndex = i;
    //     }
        
    //     _resolutionsDropdown.AddOptions(options);
    //     _resolutionsDropdown.value = currentResIndex;
    //     _resolutionsDropdown.RefreshShownValue();
    // }

    public void SetMasterVolume(float masterVolume)
    {
        _mainMixer.SetFloat("MasterVolume", masterVolume);
        _mainMixer.SetFloat("SoundsVolume", masterVolume);
        _mainMixer.SetFloat("MusicVolume", masterVolume);
    }

    public void SetSoundsVolume(float soundsVolume) => _mainMixer.SetFloat("SoundsVolume", soundsVolume);

    public void SetMusicVolume(float musicVolume) => _mainMixer.SetFloat("MusicVolume", musicVolume);

    public void SetQuality(int qualityIndex) => QualitySettings.SetQualityLevel(qualityIndex);

    public void SetFullscreen(bool isFullscreen) => Screen.fullScreen = isFullscreen;

    public void SetVSync(bool isVSync) {
        if (isVSync)
            QualitySettings.vSyncCount = 1;
            else QualitySettings.vSyncCount = 0;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = _resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}