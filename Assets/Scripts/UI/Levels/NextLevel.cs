#pragma warning disable CS0649

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
        player.CharacterBattleStats.ApplyStatsTo(GameManager.instance.playerStats.CharacterBattleStats);
        player.Inventory.ApplyItemsTo(GameManager.instance.playerStats);
        GameManager.instance.levelNumber++;
        SceneManager.LoadScene("Shop");
    }
}
