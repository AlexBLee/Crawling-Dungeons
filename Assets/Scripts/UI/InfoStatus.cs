using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoStatus : MonoBehaviour
{
    Player player;
    public BattleManager battleManager;
    
    public TextMeshProUGUI nameOfObject;
    public TextMeshProUGUI health;
    public TextMeshProUGUI mana;

    private void Start() 
    {
        player = GameManager.instance.playerObject;
        battleManager.infoStatus = this;
        UpdateText();
    }

    public void UpdateText()
    {
        nameOfObject.text = player.gameObject.name;
        health.text = "Health: " + player.hp.ToString() + "/" + player.maxHP.ToString();
        mana.text = "Mana: " + player.mp.ToString() + "/" + player.maxMP.ToString();
    }
}
