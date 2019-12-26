using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Player player;

    private void Start()
    {
        button.onClick.AddListener(GoToNextLevel);
    }

    private void GoToNextLevel()
    {
        player.ApplyStatsTo(GameManager.instance.playerStats);
        player.inventory.ApplyItemsTo(GameManager.instance.playerStats);
        GameManager.instance.levelNumber++;
        SceneManager.LoadScene("Shop");
    }
}
