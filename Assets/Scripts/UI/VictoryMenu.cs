using UnityEngine;
using UnityEngine.UI;

public class VictoryMenu : GenericMenu
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _quitButton;

    private void OnMenuButtonClick()
    {
        RemoveAllListeners();
        UIManager.Instance.LoadMenu();
    }

    private void OnQuitButtonClick()
    {
        RemoveAllListeners();
        Application.Quit();
    }

    protected override void RemoveAllListeners()
    {
        _menuButton.onClick.RemoveAllListeners();
        _quitButton.onClick.RemoveAllListeners();
    }

    protected override void AddAllListeners()
    {
        _menuButton.onClick.AddListener(OnMenuButtonClick);
        _quitButton.onClick.AddListener(OnQuitButtonClick);
    }
}
