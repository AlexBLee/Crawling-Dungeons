using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: If we want to add additional features to each screen, we should separate the screen into its own class.
public class ScreenPanelHUD : HUDMenu
{
    public GameObject pauseScreen;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void ShowGameWin()
    {
        winPanel.SetActive(true);
    }

    public void ShowPause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
