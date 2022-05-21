using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _startButton.onClick.AddListener(OnPlayButtonClick);
        _optionsButton.onClick.AddListener(OnSettingsButtonClick);
        _exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnPlayButtonClick()
    {
        UIManager.Instance.LoadGame();
    }

    private void OnSettingsButtonClick()
    {
        UIManager.Instance.ShowSettingsScreen();
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
    }
}