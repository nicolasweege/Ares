using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIOptionsMenuController : Singleton<UIOptionsMenuController> {
    [Header("Audio")]
    [SerializeField] private float _defaultVolume = 1;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private TMP_Text _musicValueText = null;
    [SerializeField] private TMP_Text _soundsValueText = null;

    [Header("Graphics")]
    [SerializeField] private Slider _fullscreenSlider;
    [SerializeField] private Slider _VSyncSlider;
    [SerializeField] private TMP_Text _fullscreenValueText = null;
    [SerializeField] private TMP_Text _VSyncValueText = null;
    private bool _isFullscreen;
    private bool _isVSync;

    #region Set Functions
    public void SetSoundsVolume(float soundsVolume) {
        AssetsManager.Instance.MainAudioMixer.SetFloat("SoundsVolume", Mathf.Log10(soundsVolume) * 20);
        _soundsValueText.text = (Math.Truncate(soundsVolume * 100) / 10).ToString();
        PlayerPrefs.SetFloat("soundsVolume", soundsVolume);
    }

    public void SetMusicVolume(float musicVolume) {
        AssetsManager.Instance.MainAudioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        _musicValueText.text = (Math.Truncate(musicVolume * 100) / 10).ToString();
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }

    public void SetFullscreen(float fullscreenValue) {
        _isFullscreen = fullscreenValue == 1 ? true : false;
        _fullscreenValueText.text = fullscreenValue == 1 ? "On" : "Off";
        PlayerPrefs.SetInt("fullscreen", (_isFullscreen ? 1 : 0));
        Screen.fullScreen = _isFullscreen;
    }

    public void SetVSync(float VSyncValue) {
        _isVSync = VSyncValue == 1 ? true : false;
        _VSyncValueText.text = VSyncValue == 1 ? "On" : "Off";
        PlayerPrefs.SetInt("vsync", (_isVSync ? 1 : 0));
        QualitySettings.vSyncCount = _isVSync ? 1 : 0;
        Application.targetFrameRate = _isVSync ? -1 : -1;
    }
    #endregion

    #region Reset Functions
    public void ResetSoundsVolume() {
        AssetsManager.Instance.MainAudioMixer.SetFloat("SoundsVolume", Mathf.Log10(_defaultVolume) * 20);
        _soundsValueText.text = (Math.Truncate(_defaultVolume * 100) / 10).ToString();
        _soundSlider.value = _defaultVolume;
    }

    public void ResetMusicVolume() {
        AssetsManager.Instance.MainAudioMixer.SetFloat("MusicVolume", Mathf.Log10(_defaultVolume) * 20);
        _musicValueText.text = (Math.Truncate(_defaultVolume * 100) / 10).ToString();
        _musicSlider.value = _defaultVolume;
    }

    public void ResetFullscreen() {
        _fullscreenValueText.text = "On";
        _fullscreenSlider.value = 1;
        Screen.fullScreen = true;
    }

    public void ResetVSync() {
        _VSyncValueText.text = "On";
        _VSyncSlider.value = 1;
        QualitySettings.vSyncCount = 1;
    }
    #endregion
}