using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelLoader : MonoBehaviour
{
    public Vector3 lastPlayerPos;
    public CharacterPanel charPanel;
    public Player player;

    
    void Awake()
    {
        lastPlayerPos = GameManager.instance.lastPlayerPos;

        //player.InitalizeStats();
        player.inBattle = false;
        player.ApplyStatsFrom(GameManager.instance.playerStats);
        player.CopyInventoryFrom(GameManager.instance.itemList);
        
        GameObject playerObject = Instantiate(player.gameObject, lastPlayerPos, Quaternion.identity);
        playerObject.name = "Player";

        GameManager.instance.playerObject = playerObject.GetComponent<Player>();

        GameManager.instance.enemyList = FindObjectsOfType<Enemy>().OrderBy(gameObject => gameObject.name).ToArray();

        GameManager.instance.addRemoves.Clear();
    }

    private void Start() 
    {
        int counter = 0;

        foreach (Enemy enemy in GameManager.instance.enemyList)
        {
            enemy.objectID = counter;
            counter++;
        }

        for (int i = 0; i < GameManager.instance.enemyList.Length; i++)
        {
            if(GameManager.instance.enemyStates[i])
            {
                GameManager.instance.enemyList[i].gameObject.SetActive(false);
            }
        }



    }


}
