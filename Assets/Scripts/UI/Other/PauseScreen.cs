using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseScreen : MonoBehaviour
{
    public Button mainMenuButton;
    public Button resumeButton;
    public Slider musicSlider;
    public Slider soundEffectSlider;

    private void Start() 
    {
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        resumeButton.onClick.AddListener(ResumeGame);

        musicSlider.value = AudioManager.Instance.GetMusicVolumeLevel();
        soundEffectSlider.value = AudioManager.Instance.GetSoundEffectVolumeLevel();
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

    public void SetMusicVolumeLevel(float sliderValue)
    {
        AudioManager.Instance.SetMusicVolumeLevel(sliderValue);
    }

    public void SetSoundEffectVolumeLevel(float sliderValue)
    {
        AudioManager.Instance.SetSoundEffectVolumeLevel(sliderValue);
    }
    
}
