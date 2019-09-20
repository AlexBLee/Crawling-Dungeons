using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public bool playerTurn = true;
    public bool battleDone = false;
    bool calculating = true;
    public Vector2 newPosition;
    public Vector2 enemyPosition;

    public Enemy enemy;
    public Player player;

    public GameObject attackButton;
    public GameObject magicButton;
    public GameObject itemButton;
    public GameObject exitButton;
    public GameObject victoryPanel;

    public GameObject background;

    public InfoStatus infoStatus;
    public VictoryInfo victoryInfo;
    public List<GameObject> spawnableEnemies;
    public int enemyCounter = 0;

    public int storedXP;

    private void Start() 
    {
        SpawnNextEnemy(enemyCounter);
        player.target = enemy;
    }

    private void Update() 
    {
        if(battleDone)
        {
            ToggleNextBattle();
        }
    }

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

    public void ToggleNextBattle()
    {
        if(calculating)
        {
            float backgroundX = background.transform.position.x;
            newPosition = new Vector2(backgroundX -= 4.0f, background.transform.position.y);
            calculating = false;
        }

        StartCoroutine(MoveToExactPosition(background.transform.position,newPosition));
        SpawnNextEnemy(enemyCounter);

    }

    public void StartNewBattle()
    {
        battleDone = false;
        player.anim.SetBool("Run", false);
        TogglePlayerTurn();
        EnableButtons();
    }

    public void SpawnNextEnemy(int number)
    {
        GameObject enemyObject = Instantiate(spawnableEnemies[number], enemyPosition, Quaternion.Euler(0,180,0));
        enemy = enemyObject.GetComponent<Enemy>();
        enemy.battleManager = this;
        enemy.target = FindObjectOfType<Player>();

        enemyCounter++;
    }
    
    protected IEnumerator MoveToExactPosition(Vector2 start, Vector2 destination)
    {
        Vector2 startPos = start;
        Vector2 endPos = destination;

        float timer = 0;
        while(timer < 2)
        {
            timer += Time.deltaTime;
            background.transform.position = Vector2.Lerp(startPos,endPos, timer/2);
            yield return null;
        }
        calculating = true;
        StartNewBattle();

    }

}
