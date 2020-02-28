using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseScreen : MonoBehaviour
{
    public Button mainMenuButton;
    public Button resumeButton;
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider soundEffectSlider;

    private void Start() 
    {
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        resumeButton.onClick.AddListener(ResumeGame);

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        soundEffectSlider.value = PlayerPrefs.GetFloat("SoundEffectVolume", 0.75f);

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
        audioMixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SetSoundEffectVolumeLevel(float sliderValue)
    {
        audioMixer.SetFloat("SoundEffect", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SoundEffectVolume", sliderValue);
    }
    
}
