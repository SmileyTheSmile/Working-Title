using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] private InputTransitionCondition _isPressingPauseSO;

    private bool _isPressingPause => _isPressingPauseSO.value;

    private void Update()
    {
        //CheckPauseInput();
    }

    private void CheckPauseInput()
    {
        if (_isPressingPause)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        GameIsPaused = true;
        UIManager.Instance.ShowPauseScreen();
        Time.timeScale = 0;
    }

    public void Resume()
    {
        GameIsPaused = false;
        UIManager.Instance.HideAllScreens();
        Time.timeScale = 1;
    }

}
