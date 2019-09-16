using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterEntity
{
    private string TAG_PLAYER = "Player";
    private GameObject player;
    public bool beginTurn;
    public float detectDistance = 10.0f;
    public int experiencePoints;

    private void Start() 
    {   
        target = GameObject.Find("Player").GetComponent<CharacterEntity>();
        initialPos = transform.position;
    }

    void Update()
    {
        if(inBattle && !battleManager.playerTurn)
        {            
            MoveAndAttack(target.transform.position, 1);
        }
        StopAttacking();
    }


    public void SetAttackConditions()
    {
        initialPos = transform.position;
        targetReached = false;
        attacking = true;
    }

    public void CheckDeath()
    {
        if(hp <= 0 && inBattle)
        {
            target.GetComponent<Player>().RecieveXP(experiencePoints);
            Destroy(gameObject);
        }
    }



}
