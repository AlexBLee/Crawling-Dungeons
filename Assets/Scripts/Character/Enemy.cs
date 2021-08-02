using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : CharacterEntity
{
    public int experiencePoints;
    public int gold;

    public bool newBattle;
    [SerializeField] private Vector3 fightPosition;
    [SerializeField] private bool canHeal;
    private int healCounter = 1;

    private const int healCounterDefault = 2;
    private const float bonusDamage = 1.25f;
    private const float enemyLowHealthPercentage = 0.40f;
    private const float selfLowHealthPercentage = 0.30f;
    private const float healFactor = 0.20f;

    private void Start() 
    {   
        initialPos = transform.position;
        uiManager = FindObjectOfType<UIManager>();
        UpdateDamageStats();

        MoveToStartPosition(fightPosition);
    }

    protected void MoveToStartPosition(Vector2 position)
    {
        anim.SetBool("Run", true);
        iTween.MoveTo(gameObject, iTween.Hash("x", position.x, "onComplete", "StartBattle"));
    }

    private void StartBattle()
    {
        anim.SetBool("Run", false);
        battleManager.StartNewBattle();
    }

    public void SetAttackConditions()
    {
        // The AI can only heal once every two turns.
        // The counter is there to keep count of how manys turns its been since it last healed.

        if(!dead)
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
        }

        
    }

    public void FindBestMove()
    {
        // Heal if self is low on health and is able to heal
        if(hp < maxHP * selfLowHealthPercentage && canHeal)
        {
            canHeal = false;
            healCounter = healCounterDefault;
            Heal((int)(maxHP * healFactor), false);
        }
        // Use spell if the player is low on health
        else if(target.hp < target.maxHP * enemyLowHealthPercentage)
        {
            additionalDamage = (int)(maxDamage * bonusDamage);
            RangedAttack();
        }
        // Normal Attack
        else
        {
            initialPos = transform.position;
            MoveToAttackPosition(target.transform.position);
        }

    }

    public override void FinishDeath()
    {
        target.GetComponent<Player>().RecieveXPAndGold(experiencePoints, gold);
        base.FinishDeath();
    }



}
