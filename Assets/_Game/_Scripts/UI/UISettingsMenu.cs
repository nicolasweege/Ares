using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System;

public class UISettingsMenu : MonoBehaviour
{
    [SerializeField] private float _defaultVolume = 1;
    [SerializeField] private AudioMixer _mainMixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private TMP_Text _musicValueText = null;
    [SerializeField] private TMP_Text _soundsValueText = null;
    private Resolution[] _resolutions;
    // [SerializeField] private Dropdown _resolutionsDropdown;

    private void Start()
    {
        // HandleResolutions();
        LoadPlayerAudioPrefs();
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

    public void SetSoundsVolume(float soundsVolume) {
        _mainMixer.SetFloat("SoundsVolume", Mathf.Log10(soundsVolume) * 20);
        PlayerPrefs.SetFloat("soundsVolume", soundsVolume);
        _soundsValueText.text = (Math.Truncate(_soundSlider.value * 100) / 10).ToString();
    }

    public void SetMusicVolume(float musicVolume) {
        _mainMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        _musicValueText.text = (Math.Truncate(_musicSlider.value * 100) / 10).ToString();
    }

    private void LoadPlayerAudioPrefs() {
        _mainMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("musicVolume", _defaultVolume)) * 20);
        _mainMixer.SetFloat("SoundsVolume", Mathf.Log10(PlayerPrefs.GetFloat("soundsVolume", _defaultVolume)) * 20);
        _musicSlider.value = PlayerPrefs.GetFloat("musicVolume", _defaultVolume);
        _soundSlider.value = PlayerPrefs.GetFloat("soundsVolume", _defaultVolume);
        _musicValueText.text = (Math.Truncate(_musicSlider.value * 100) / 10).ToString();
        _soundsValueText.text = (Math.Truncate(_soundSlider.value * 100) / 10).ToString();
    }

    public void SetQuality(int qualityIndex) => QualitySettings.SetQualityLevel(qualityIndex);

    public void SetFullscreen(bool isFullscreen) => Screen.fullScreen = isFullscreen;

    public void SetVSync(bool isVSync) {
        if (isVSync)
            QualitySettings.vSyncCount = 4;
            else QualitySettings.vSyncCount = 0;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = _resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}