using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _exitButton;

    private void OnEnable()
    {
        AddAllListeners();
    }

    private void OnDisable()
    {
        RemoveAllListeners();
    }

    private void OnPlayButtonClick()
    {
        RemoveAllListeners();
        UIManager.Instance.LoadGame();
    }

    private void OnOptionsButtonClick()
    {
        RemoveAllListeners();
        UIManager.Instance.ShowOptionsScreen();
    }

    private void OnExitButtonClick()
    {
        RemoveAllListeners();
        Application.Quit();
    }

    private void RemoveAllListeners()
    {
        _startButton.onClick.RemoveAllListeners();
        _optionsButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }

    private void AddAllListeners()
    {
        _startButton.onClick.AddListener(OnPlayButtonClick);
        _optionsButton.onClick.AddListener(OnOptionsButtonClick);
        _exitButton.onClick.AddListener(OnExitButtonClick);
    }
}