using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public Button button;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(GoToNextLevel);
    }

    void GoToNextLevel()
    {
        player.ApplyStatsTo(GameManager.instance.playerStats);
        player.inventory.ApplyItemsTo(GameManager.instance.playerStats);
        GameManager.instance.levelNumber++;
        SceneManager.LoadScene("Shop");
    }
}
