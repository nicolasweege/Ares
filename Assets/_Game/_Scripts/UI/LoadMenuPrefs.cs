using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class LoadMenuPrefs : MonoBehaviour
{
    [SerializeField] private bool _canUse = false;

    [Header("Audio")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private TMP_Text _musicValueText = null;
    [SerializeField] private TMP_Text _soundsValueText = null;

    [Header("Graphics")]
    [SerializeField] private Slider _fullscreenSlider;
    [SerializeField] private Slider _VSyncSlider;
    [SerializeField] private TMP_Text _fullscreenValueText = null;
    [SerializeField] private TMP_Text _VSyncValueText = null;

    private void Start() {
        LoadGraphicsPrefs();
        LoadAudioPrefs();
    }

    private void LoadAudioPrefs() {
        if (_canUse) {
            if (PlayerPrefs.HasKey("musicVolume")) {
                float localVolumeValue = PlayerPrefs.GetFloat("musicVolume");
                _musicValueText.text = (Math.Truncate(localVolumeValue * 100) / 10).ToString();
                _musicSlider.value = localVolumeValue;
                AssetsManager.Instance.MainAudioMixer.SetFloat("MusicVolume", Mathf.Log10(localVolumeValue) * 20);
            }
            else UIOptionsMenuController.Instance.ResetMusicVolume();

            if (PlayerPrefs.HasKey("soundsVolume")) {
                float localVolumeValue = PlayerPrefs.GetFloat("soundsVolume");
                _soundsValueText.text = (Math.Truncate(localVolumeValue * 100) / 10).ToString();
                _soundSlider.value = localVolumeValue;
                AssetsManager.Instance.MainAudioMixer.SetFloat("SoundsVolume", Mathf.Log10(localVolumeValue) * 20);
            }
            else UIOptionsMenuController.Instance.ResetSoundsVolume();
        }
    }

    private void LoadGraphicsPrefs() {
        if (_canUse) {
            // Fullscreen
            if (PlayerPrefs.HasKey("fullscreen")) {
                int fullscreenLocalValue = PlayerPrefs.GetInt("fullscreen");

                if (fullscreenLocalValue == 1) {
                    Screen.fullScreen = true;
                    _fullscreenValueText.text = "On";
                    _fullscreenSlider.value = 1;
                }
                else {
                    Screen.fullScreen = false;
                    _fullscreenValueText.text = "Off";
                    _fullscreenSlider.value = 0;
                }
            }
            else UIOptionsMenuController.Instance.ResetFullscreen();

            // VSync
            if (PlayerPrefs.HasKey("vsync")) {
                int localVSyncValue = PlayerPrefs.GetInt("vsync");

                if (localVSyncValue == 1) {
                    QualitySettings.vSyncCount = 1;
                    _VSyncValueText.text = "On";
                    _VSyncSlider.value = 1;
                }
                else {
                    QualitySettings.vSyncCount = 0;
                    _VSyncValueText.text = "Off";
                    _VSyncSlider.value = 0;
                }
            }
            else UIOptionsMenuController.Instance.ResetVSync();
        }
    }
}