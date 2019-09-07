using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public bool playerTurn = true;
    public Enemy enemy;

    public GameObject attackButton;
    public GameObject exitButton;
    public GameObject victoryPanel;
    public AttackHeader attackHeader;
    public InfoStatus infoStatus;
    public VictoryInfo victoryInfo;
    public List<GameObject> battleList;

    public int storedXP;



    public void TogglePlayerTurn()
    {
        playerTurn = true;
    }

    public void ToggleEnemyTurn()
    {
        playerTurn = false;
    }

    public void ToggleNextTurn()
    {
        if(playerTurn)
        {
            if(enemy != null)
            {
                enemy.SetAttackConditions();
            }
            playerTurn = false;
        }
        else
        {
            attackButton.SetActive(true);
            playerTurn = true;
        }
    }

    public void ToggleBattleEnd()
    {
        SceneManager.LoadScene("TestLevel");
    }

    public void CheckBattleList()
    {
        if(battleList.Count == 0)
        {
            Player playerA = FindObjectOfType<Player>();
            playerA.RecieveXP(storedXP);
            playerA.ApplyStatsTo(GameManager.instance.playerStats);
            playerA.CopyInventoryTo(GameManager.instance.playerStats.itemList);

            exitButton.GetComponent<Button>().onClick.AddListener(ToggleBattleEnd);
            victoryPanel.SetActive(true);
        }
    }
}
