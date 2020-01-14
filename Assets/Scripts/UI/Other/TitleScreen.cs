using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    void Update()
    {
        #if UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            SceneManager.LoadScene("Level1");
        }
        #endif

        #if UNITY_EDITOR || UNITY_STANDALONE
        if(Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene("Level1");
        }
        #endif
    }
}
