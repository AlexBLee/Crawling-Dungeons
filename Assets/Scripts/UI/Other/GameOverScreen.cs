using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {
            GameManager.instance.ResetGame();
            SceneManager.LoadScene("Level1");
        }
    }
}
