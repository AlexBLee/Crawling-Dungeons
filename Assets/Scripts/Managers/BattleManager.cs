#pragma warning disable CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public bool playerTurn = true;
    public bool battleDone = false;
    private bool newBattle = true;
    private bool calculating = true;
    private bool needToSpawn = true;
    private Vector2 newPosition;
    private Vector2 enemyPosition = new Vector2(6.88f, 2.68f);

    [SerializeField] private Enemy enemy;
    [SerializeField] private Player player;
    
    [SerializeField, HideInInspector] private UIManager uiManager;
    [SerializeField, HideInInspector] private GameObject background;

    public List<GameObject> spawnableEnemies;
    private int enemyCounter = 0;

    private void Start() 
    {
        uiManager.DisableButtons();
        SpawnNextEnemy(enemyCounter);
        player.target = enemy;
        uiManager.victoryPanel.SetActive(false);
    }

    private void Update() 
    {
        if(battleDone)
        {
            ToggleNextBattle();
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
        if(playerTurn)
        {
            if(enemy != null)
            {
                yield return new WaitForSeconds(0.5f);
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

        if(calculating)
        {
            float backgroundX = background.transform.position.x;
            newPosition = new Vector2(backgroundX -= 4.0f, background.transform.position.y);
            calculating = false;
        }

        if(needToSpawn)
        {
            SpawnNextEnemy(enemyCounter);
            needToSpawn = false;
        }

        StartCoroutine(MoveToExactPosition(background.transform.position,newPosition));


    }

    public void StartNewBattle()
    {
        battleDone = false;
        player.anim.SetBool("Run", false);
        TogglePlayerTurn();
        needToSpawn = true;
        newBattle = true;
    }

    private void SpawnNextEnemy(int number)
    {
        if(enemyCounter < spawnableEnemies.Count)
        {
            GameObject enemyObject = Instantiate(spawnableEnemies[number], enemyPosition, Quaternion.Euler(0,180,0));
            enemy = enemyObject.GetComponent<Enemy>();
            enemy.battleManager = this;
            enemy.target = FindObjectOfType<Player>();
            enemy.newBattle = true;
            player.target = enemy;

            needToSpawn = false;
            enemyCounter++;
        }
        // If no more enemies to spawn, floor is complete.
        else
        {
            AudioManager.Instance.Play("Victory");
            uiManager.victoryPanel.SetActive(true);
            
            if(player.statPoints == 0)
            {
                uiManager.DeactivateAdders();
            }
        }
    }
    
    private IEnumerator MoveToExactPosition(Vector2 start, Vector2 destination)
    {
        Vector2 startPos = start;
        Vector2 endPos = destination;

        float timer = 0;
        while(timer < 2)
        {
            timer += Time.deltaTime;
            background.transform.position = Vector2.Lerp(startPos,endPos, timer/1.20f);
            yield return null;
        }

        if(newBattle)
        {
            calculating = true;
            newBattle = false;
        }

    }

}
