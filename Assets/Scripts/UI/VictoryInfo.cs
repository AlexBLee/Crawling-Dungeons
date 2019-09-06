using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryInfo : MonoBehaviour
{
    public BattleManager battleManager;

    public TextMeshProUGUI expGained;
    public TextMeshProUGUI itemObtained;
    public TextMeshProUGUI nameOfCharacter;
    public TextMeshProUGUI currentLevel;

    private string exp;
    public string playerLevel;
    public string playerName;

    private void Start() 
    {
        exp = battleManager.storedXP.ToString();
        playerName = GameManager.instance.playerObject.gameObject.name;
        battleManager.victoryInfo = this;
        ShowText();
    }

    public void ShowText()
    {
        expGained.text = "EXP: " + exp;
        itemObtained.text = "Items Obtained: ";
        nameOfCharacter.text = playerName;
        currentLevel.text = "Level: " + GameManager.instance.playerLevel;
    }
}
