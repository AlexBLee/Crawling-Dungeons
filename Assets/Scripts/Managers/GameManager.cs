using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player playerStats;
    public int levelNumber = 2;

    // static as it needs to be accessed from title screen without GameManager existing in the menu.
    public static bool endlessMode = false;
    public int endlessNumber = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(playerStats.gameObject);

        // GameManager needs a copy of some player in the scene.. Looking to change this..
        playerStats.gameObject.SetActive(true);

        ResetGame();


        playerStats.gameObject.SetActive(false);
    }

    public void ResetGame()
    {
        playerStats.InitalizeStats();
        playerStats.inventory.InitializeInventory();
        levelNumber = 1;
    }



    



}
