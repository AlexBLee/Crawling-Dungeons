#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private GameObject tapToStart;
    [SerializeField] private Button standardButton;
    [SerializeField] private Button endlessButton;
    [SerializeField] private Button quitButton;

    private void Start() 
    {
        standardButton.onClick.AddListener(PlayStandard);
        endlessButton.onClick.AddListener(PlayEndless);
        quitButton.onClick.AddListener(Quit);
    }

    void Update()
    {
        #if UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            tapToStart.SetActive(false);
            standardButton.gameObject.SetActive(true);
            endlessButton.gameObject.SetActive(true);
        }
        #endif

        #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if(Input.GetKey(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            tapToStart.SetActive(false);
            standardButton.gameObject.SetActive(true);
            endlessButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
        }
        #endif
    }

    private void PlayStandard()
    {
        SceneManager.LoadScene("Level1");
    }

    private void PlayEndless()
    {
        SceneManager.LoadScene("Level1");
        GameManager.endlessMode = true;
    }

    private void Quit()
    {
        Application.Quit();
    }
}
