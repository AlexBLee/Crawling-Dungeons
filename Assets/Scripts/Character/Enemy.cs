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
        // The AI can only heal once every two turns.
        // The counter is there to keep count of how manys turns its been since it last healed.
        if(!canHeal)
        {
            healCounter--;
        }
        if(healCounter == 0)
        {
            canHeal = true;
        }

        FindBestMove();

        
    }

    public void FindBestMove()
    {
        // Heal if self is low on health and is able to heal
        if(hp < maxHP * 0.30f && hpCounter > 0 && canHeal)
        {
            hpCounter--;
            canHeal = false;
            healCounter = 2;
            UseHealthItem(uiManager.healthPot);
        }
        // Use spell if the player is low on health
        else if(target.hp < target.maxHP * 0.60f)
        {
            additionalDamage = (int)maxDamage * 2;
            RangedAttack();
        }
        // Normal Attack
        else
        {
            initialPos = transform.position;
            targetReached = false;
            attacking = true;
        }

    }

    public override void CheckDeath()
    {
        if(hp <= 0 && inBattle)
        {
            anim.SetTrigger("Death");
        }
    }

    public void FinishDeath()
    {
        target.GetComponent<Player>().RecieveXP(experiencePoints);
        Destroy(gameObject);
    }



}
