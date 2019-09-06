using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSpawner : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    public BattleManager battleManager;

    public Transform playerPosition;
    public Transform enemyPosition;



    private void Start()
    {
        // Battle UI
        battleManager.victoryPanel.SetActive(false);
        battleManager.attackHeader.gameObject.SetActive(false);

        // Player
        player.ApplyStatsFrom(GameManager.instance.playerStats);
        player.CopyInventoryFrom(GameManager.instance.itemList);
        player.inBattle = true;
        GameManager.instance.playerStats = player;

        // Enemy
        enemy = FindObjectOfType<Enemy>();
        battleManager.enemy = enemy;
        enemy.inBattle = true;
        enemy.UpdateDamageStats();
        enemy.anim.SetBool("Run", false);
        GameManager.instance.enemyStats = enemy;
        

        // Spawning
        GameObject playerObj = Instantiate(player.gameObject, playerPosition.position, Quaternion.Euler(0,90,0));
        GameManager.instance.playerObject = playerObj.GetComponent<Player>();
        enemy.gameObject.transform.position = enemyPosition.position;
        enemy.gameObject.transform.rotation = Quaternion.Euler(0,-90,0);

        // XP Calc
        enemy.target = playerObj.GetComponent<CharacterEntity>();
        battleManager.storedXP = (Mathf.Max(enemy.target.level, enemy.level) - Mathf.Min(enemy.target.level, enemy.level)) * 250;
        enemy.beginTurn = true;
        
        // Naming
        playerObj.name = "Player";
        enemy.gameObject.name = "Enemy";

        // List
        battleManager.battleList.Add(enemy.gameObject);




    }

}
