﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Fader _fader;
    [SerializeField] private GameObject _menuScreen;
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private GameObject _settingsScreen;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ShowMenuScreen();
    }

    public void LoadMenu()
    {
        _fader.OnFadeOut += LoadMenuScene;
        _fader.FadeOut();
    }

    public void LoadGame()
    {
        _fader.OnFadeOut += LoadGameScene;
        _fader.FadeOut();
    }

    private void LoadMenuScene()
    {
        _fader.OnFadeOut -= LoadMenuScene;
        StartCoroutine(LoadSceneCoroutine("Menu"));
        ShowMenuScreen();
    }

    private void LoadGameScene()
    {
        _fader.OnFadeOut -= LoadGameScene;
        StartCoroutine(LoadSceneCoroutine("Game"));
        ShowGameScreen();
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone) {
            yield return null;
        }
        _fader.FadeIn();
    }

    public void ShowSettingsScreen()
    {
        HideAllScreens();
        _settingsScreen.SetActive(true);
    }

    public void ShowMenuScreen()
    {
        HideAllScreens();
        _menuScreen.SetActive(true);
    }

    public void ShowGameScreen()
    {
        HideAllScreens();
        _gameScreen.SetActive(true);
    }

    public void HideAllScreens()
    {
        _menuScreen.SetActive(false);
        _gameScreen.SetActive(false);
        _settingsScreen.SetActive(false);
    }
}