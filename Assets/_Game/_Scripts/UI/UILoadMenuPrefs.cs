using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UILoadMenuPrefs : MonoBehaviour
{
    [SerializeField] private bool _canUse = false;
    [SerializeField] private UIMainMenu _mainMenuController;
    [SerializeField] private UISettingsMenu _settingsMenuController;

    [Header("Audio")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private TMP_Text _musicValueText = null;
    [SerializeField] private TMP_Text _soundsValueText = null;

    private void Awake() {
        LoadAudioPrefs();
    }

    private void LoadAudioPrefs() {
        if (_canUse) {
            if (PlayerPrefs.HasKey("musicVolume")) {
                float localVolume = PlayerPrefs.GetFloat("musicVolume");
                _musicValueText.text = (Math.Truncate(localVolume * 100) / 10).ToString();
                _musicSlider.value = localVolume;
                AssetsManager.Instance.MainAudioMixer.SetFloat("MusicVolume", Mathf.Log10(localVolume) * 20);
            }
            else {
                _settingsMenuController.ResetMusicVolume();
            }

            if (PlayerPrefs.HasKey("soundsVolume")) {
                float localVolume = PlayerPrefs.GetFloat("soundsVolume");
                _soundsValueText.text = (Math.Truncate(localVolume * 100) / 10).ToString();
                _soundSlider.value = localVolume;
                AssetsManager.Instance.MainAudioMixer.SetFloat("SoundsVolume", Mathf.Log10(localVolume) * 20);
            }
            else {
                _settingsMenuController.ResetSoundsVolume();
            }
        }
    }
}