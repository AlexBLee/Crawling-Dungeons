using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        #if UNITY_ANDROID
        if(Input.GetKey(KeyCode.Return))
        {
            GameManager.instance.ResetGame();
            SceneManager.LoadScene("Level1");
        }
        #endif

        #if UNITY_EDITOR || UNITY_STANDALONE
        if(Input.GetKey(KeyCode.Return))
        {
            GameManager.instance.ResetGame();
            SceneManager.LoadScene("Level1");
        }
        #endif
    }
}
