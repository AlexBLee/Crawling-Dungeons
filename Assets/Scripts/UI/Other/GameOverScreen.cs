using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        #if UNITY_ANDROID
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            GameManager.instance.ResetGame();
            SceneManager.LoadScene("Level1");
        }
        #endif

        #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if(Input.GetKey(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            GameManager.instance.ResetGame();
            SceneManager.LoadScene("Level1");
        }
        #endif
    }
}
