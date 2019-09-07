using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterEntity
{
    private string TAG_PLAYER = "Player";
    private GameObject player;
    public bool beginTurn;
    public float detectDistance = 10.0f;

    private void Start() 
    {   
        player = GameObject.Find("Player");



    }

    void Update()
    {
        // if(inBattle && !GameManager.instance.battleManager.playerTurn)
        // {            
        //     MoveAndAttack(target.transform.position, 1);
        //     StopAttacking();
        // }


        // if(!inBattle)
        // {
        //     if(Vector3.Distance(gameObject.transform.position, player.transform.position) < detectDistance)
        //     {
        //         transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        //         transform.LookAt(player.transform.position);
        //         anim.SetBool("Run", true);
        //     }
        //     else
        //     {
        //         anim.SetBool("Run", false);
        //     }

        // }


        
    }


    public void SetAttackConditions()
    {
        initialPos = transform.position;
        targetReached = false;
        targetChosen = true;
        attacking = true;
    }

    public void CheckDeath()
    {
        if(hp <= 0 && inBattle)
        {       
            GameManager.instance.battleManager.battleList.RemoveAt(GameManager.instance.battleManager.battleList.IndexOf(gameObject));
            GameManager.instance.enemyStates[objectID] = true;
            ApplyStatsTo(GameManager.instance.enemyStats);
            Destroy(gameObject);
            GameManager.instance.battleManager.CheckBattleList();
        }
    }



}
