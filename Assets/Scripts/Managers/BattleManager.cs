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
    public GameObject magicButton;
    public GameObject itemButton;
    public GameObject exitButton;
    public GameObject victoryPanel;

    public InfoStatus infoStatus;
    public VictoryInfo victoryInfo;
    public List<GameObject> battleList;

    public int storedXP;

    public void DisableButtons()
    {
        attackButton.SetActive(false);
        magicButton.SetActive(false);
        itemButton.SetActive(false);
    }

    public void EnableButtons()
    {
        attackButton.SetActive(true);
        magicButton.SetActive(true);
        itemButton.SetActive(true);
    }

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
            EnableButtons();
            playerTurn = true;
        }
    }
}
