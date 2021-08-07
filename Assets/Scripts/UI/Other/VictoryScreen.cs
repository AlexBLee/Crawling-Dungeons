﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        #if UNITY_ANDROID
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if(GameManager.endlessMode)
            {
                GameManager.instance.endlessNumber++;
            }
            else
            {
                GameManager.instance.ResetGame();
            }
            SceneManager.LoadScene("Level1");
        }
        #endif

        #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if(Input.GetKey(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            if(GameManager.endlessMode)
            {
                GameManager.instance.endlessNumber++;
                GameManager.instance.levelNumber = 1;
            }
            else
            {
                GameManager.instance.ResetGame();
            }
            SceneManager.LoadScene("Level1");
        }
        #endif
    }
}
