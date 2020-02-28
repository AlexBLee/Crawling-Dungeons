using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public Button mainMenuButton;
    public Button resumeButton;

    private void Start() 
    {
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        resumeButton.onClick.AddListener(ResumeGame);
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }

    private void ResumeGame()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
    
}
