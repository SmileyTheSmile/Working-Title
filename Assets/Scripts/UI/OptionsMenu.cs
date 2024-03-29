﻿using UnityEngine;
using UnityEngine.UI;
using System;

public class OptionsMenu : GenericMenu
{
    [Serializable]
    public class Settings
    {
        public float volumeMaster = 0.5f;
        public float menuVolume = 0.5f;
    }

    [SerializeField] private Button _okButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Slider _menuVolumeSlider;

    [SerializeField] private MenuAudio _menuAudio;

    private Settings _settings = new Settings();
    private const string RECORDS_KEY = "settings";

    private void Awake()
    {
        LoadFromPlayerPrefs();
        UpdateSliderValues();
        SetVolume(_settings.menuVolume);
    }

    private void OnOkButtonClick()
    {
        SaveSettings();
        ExitToMenu();
    }

    private void OnCancelButtonClick()
    {
        ExitToMenu();
    }

    private void ExitToMenu()
    {
        UpdateSliderValues();
        UIManager.Instance.ShowMenuScreen();
    }

    private void SaveSettings()
    {
        SaveToPlayerPrefs();
    }

    private void SaveToPlayerPrefs()
    {
        SetNewSettings();
        var json = JsonUtility.ToJson(_settings);
        PlayerPrefs.SetString(RECORDS_KEY, json);
    }

    private void LoadFromPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey(RECORDS_KEY))
            return;

        var newSettings = JsonUtility.FromJson<Settings>(PlayerPrefs.GetString(RECORDS_KEY));
        _settings = newSettings;
    }

    private void SetNewSettings()
    {
        _settings.menuVolume = _menuVolumeSlider.value;
    }

    private void UpdateSliderValues()
    {
        _menuVolumeSlider.value = _settings.menuVolume;
    }

    private void SetVolume(float value)
    {
        _menuAudio.SetVolume(value);
    }

    protected override void AddAllListeners()
    {
        _okButton.onClick.AddListener(OnOkButtonClick);
        _backButton.onClick.AddListener(OnCancelButtonClick);
        _menuVolumeSlider.onValueChanged.AddListener(SetVolume);
    }

    protected override void RemoveAllListeners()
    {
        _okButton.onClick.RemoveAllListeners();
        _backButton.onClick.RemoveAllListeners();
        _menuVolumeSlider.onValueChanged.RemoveAllListeners();
    }
}