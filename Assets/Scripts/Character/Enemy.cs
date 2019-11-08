using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : CharacterEntity
{
    public int experiencePoints;
    public bool newBattle;
    public Vector3 fightPosition;

    public bool canHeal;
    public int healCounter = 1;

    private void Start() 
    {   
        initialPos = transform.position;
        uiManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        if(newBattle)
        {
            MoveToPosition();
        }

        if(inBattle && !battleManager.playerTurn)
        {            
            MoveAndAttack(target.transform.position, 1);
        }
        StopAttacking();
    }

    public void MoveToPosition()
    {
        if (transform.position != fightPosition)
        {
            StartCoroutine(MoveToExactPosition(fightPosition));
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
            newBattle = false;
            battleManager.StartNewBattle();
        }
    }

    public void SetAttackConditions()
    {
        if(!canHeal)
        {
            healCounter--;
        }
        if(healCounter == 0)
        {
            canHeal = true;
        }

        FindBestMove();


        initialPos = transform.position;
        targetReached = false;
        attacking = true;
    }

    public void FindBestMove()
    {
        if(hp < maxHP*0.30f && hpCounter > 0 && canHeal)
        {
            hpCounter--;
            canHeal = false;
            healCounter = 2;
            Debug.Log("HEAL!");
        }
        else if(target.hp < target.maxHP*0.60f)
        {
            Debug.Log("use spell!");
        }
        else
        {
            Debug.Log("ATTACK!");
        }

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
