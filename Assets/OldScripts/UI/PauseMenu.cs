using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : GenericMenu
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _menuButton;

    private void OnResumeButtonClick()
    {
        RemoveAllListeners();
    }

    private void OnOptionsButtonClick()
    {
        RemoveAllListeners();
        UIManager.Instance.ShowOptionsScreen();
    }

    private void OnMenuButtonClick()
    {
        RemoveAllListeners();
        UIManager.Instance.LoadMenu();
    }

    protected override void RemoveAllListeners()
    {
        _resumeButton.onClick.RemoveAllListeners();
        _optionsButton.onClick.RemoveAllListeners();
        _menuButton.onClick.RemoveAllListeners();
    }

    protected override void AddAllListeners()
    {
        _resumeButton.onClick.AddListener(OnResumeButtonClick);
        _optionsButton.onClick.AddListener(OnOptionsButtonClick);
        _menuButton.onClick.AddListener(OnMenuButtonClick);
    }
}
