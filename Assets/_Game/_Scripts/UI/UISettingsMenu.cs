using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UISettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private float _defaultVolume = 1;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private TMP_Text _musicValueText = null;
    [SerializeField] private TMP_Text _soundsValueText = null;

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

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    public void SetVSync(bool isVSync) {
        if (isVSync)
            QualitySettings.vSyncCount = 4;
            else QualitySettings.vSyncCount = 0;
    }
}