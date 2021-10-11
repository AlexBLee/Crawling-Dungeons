#pragma warning disable CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public bool playerTurn = true;

    [SerializeField] private Enemy enemy;
    [SerializeField] private Player player;
    
    [SerializeField] private UIManager uiManager;

    public List<Enemy> enemyList;
    public List<Enemy> spawnableEnemies;
    private int enemyCounter = 0;

    private void Start() 
    {
        player = FindObjectOfType<Player>();

        uiManager.DisableButtons();
        InitializeEnemiesForBattle();
        SpawnNextEnemy(enemyCounter);
        player.target = enemy;
    }

    private void InitializeEnemiesForBattle()
    {
        spawnableEnemies.Clear();

        string[] levelData = GameDatabase.instance.GetLevelData(GameManager.instance.levelNumber);
        foreach(string enemyName in levelData)
        {
            Enemy enemyFound = enemyList.Find(enemy => enemy.name == enemyName);

            if (enemyFound == null)
            {
                continue;
            }

            spawnableEnemies.Add(enemyFound);
        }
    }

    private void TogglePlayerTurn()
    {
        uiManager.EnableButtons();
        playerTurn = true;
    }
    
    // Only for the spells as the animation triggers cannot start coroutines themselves.
    public void BeginWait()
    {
        StartCoroutine(ToggleNextTurn());
    }

    public IEnumerator ToggleNextTurn()
    {
        const float BeginAttackDelay = 0.8f;

        if(playerTurn)
        {
            if(enemy != null)
            {
                yield return new WaitForSeconds(BeginAttackDelay);
                uiManager.DisableButtons();
                enemy.SetAttackConditions();
            }
            playerTurn = false;
        }
        else
        {
            uiManager.EnableButtons();
            playerTurn = true;
        }
    }

    public void ToggleNextBattle()
    {
        SpawnNextEnemy(enemyCounter);
        uiManager.MoveBackgroundToPosition();
    }

    public void StartNewBattle()
    {
        player.anim.SetBool("Run", false);
        TogglePlayerTurn();
    }

    private void SpawnNextEnemy(int number)
    {
        Vector2 enemyPosition = new Vector2(6.88f, 2.68f);
        Vector2 reverseRotation = new Vector2(0,180);

        if(enemyCounter < spawnableEnemies.Count)
        {
            GameObject enemyObject = Instantiate(spawnableEnemies[number].gameObject, enemyPosition, Quaternion.Euler(reverseRotation));
            enemy = enemyObject.GetComponent<Enemy>();

            if(GameManager.endlessMode)
            {
                BuffEndlessEnemyStats(enemy);
            }
            
            enemy.battleManager = this;
            enemy.target = FindObjectOfType<Player>();
            enemy.newBattle = true;
            player.target = enemy;

            enemyCounter++;
        }
        // If no more enemies to spawn, floor is complete.
        else
        {
            DeclareVictory();
        }
    }

    public void DeclareVictory()
    {
        const int FinalLevelNumber = 7;

        AudioManager.Instance.PlayMusic(AudioStrings.VictoryMusic);
        if (SceneManager.GetActiveScene().buildIndex == FinalLevelNumber)
        {
            uiManager.ShowGameWin();
        }
        else
        {
            uiManager.ShowVictoryPanel();
            player.DeclareVictory();
        }
    }
    
    public void BuffEndlessEnemyStats(Enemy enemy)
    {
        const float EndlessModeFactor = 1.25f;

        float x = Mathf.Pow(EndlessModeFactor, GameManager.instance.endlessNumber);
        
        enemy.hp = (int)(enemy.hp * x);
        enemy.mp = (int)(enemy.mp * x);
        enemy.maxHP = (int)(enemy.maxHP * x);
        enemy.maxMP = (int)(enemy.maxMP * x);
        enemy.str.amount = (int)(enemy.str.amount * x);
        enemy.dex.amount = (int)(enemy.dex.amount * x);
        enemy.intl.amount = (int)(enemy.intl.amount * x);
        enemy.luck.amount = (int)(enemy.luck.amount * x);
        
    }
}
