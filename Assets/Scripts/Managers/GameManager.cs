using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player playerStats;
    public int levelNumber = 2;

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
    }

    private void Start() 
    {
        playerStats.gameObject.SetActive(true);

        playerStats.InitalizeStats();

        playerStats.gameObject.SetActive(false);
    }


    



}
